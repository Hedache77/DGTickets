using DGTickets.Backend.Helpers;
using DGTickets.Backend.UnitsOfWork.Implementations;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IFileStorage fileStorage, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _fileStorage = fileStorage;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
        await CheckMedicinesAsync();
        await CheckHeadquartersAsync();
        await CheckRatingsAsync();
        await CheckModulesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("DG", "Tickets", "admin@yopmail.com", "322 311 4620", UserType.Admin);
        await CheckUserAsync("DG", "Tickets", "asesor@yopmail.com", "322 310 4620", UserType.Adviser);
        await CheckUserAsync("DG", "Tickets", "usuario@yopmail.com", "322 310 4620", UserType.User);
        await CheckPQRsAsync();
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            await CheckFlagsCountriesAsync();
        }
    }

    private async Task CheckStatesAsync()
    {
        if (!_context.States.Any())
        {
            var statesSQLScript = File.ReadAllText("Data\\States.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSQLScript);
            await CheckFlagsStatesAsync();
        }
    }

    private async Task CheckCitiesAsync()
    {
        if (!_context.Cities.Any())
        {
            var citiesSQLScript = File.ReadAllText("Data\\Cities.sql");
            await _context.Database.ExecuteSqlRawAsync(citiesSQLScript);
            await CheckImagesCitiesAsync();
        }
    }

    private async Task CheckMedicinesAsync()
    {
        if (!_context.MedicinesStock.Any())
        {
            var medicinesSQLScript = File.ReadAllText("Data\\Medicines.sql");
            await _context.Database.ExecuteSqlRawAsync(medicinesSQLScript);
            await CheckImagesMedicineAsync();
        }
    }

    private async Task CheckHeadquartersAsync()
    {
        if (!_context.HeadquarterMedicines.Any())
        {
            var acetaminofen = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "ACETAMINOFEN")!;
            var dolex = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Dolex");
            var ibuprofeno = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Ibuprofeno");
            var sevedol = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Sevedol");
            var electrolit = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Electrolit");
            var tramadol = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Tramadol");
            var naproxeno = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Naproxeno");
            var loratadina = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Loratadina");
            var amoxicilina = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Amoxicilina");
            var buscapina = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Buscapina");
            var minoxidil = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Minoxidil");
            var advil = await _context.MedicinesStock.FirstOrDefaultAsync(x => x.Name == "Advil");

            var center = new Headquarter
            {
                Name = "Centro",
                Address = "Avenida Princial",
                PhoneNumber = "3100000000",
                Email = "centro@yopmail.com",
                CityId = 1,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = sevedol! },
                    new HeadquarterMedicine { Medicine = electrolit! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = minoxidil! },
                ]
            };

            var pub = new Headquarter
            {
                Name = "Plaza",
                Address = "Plaza Mayor",
                PhoneNumber = "3000000000",
                Email = "plaza@yopmail.com",
                CityId = 2,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = ibuprofeno! },
                    new HeadquarterMedicine { Medicine = naproxeno! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = advil! },
                ]
            };

            var park = new Headquarter
            {
                Name = "Parque Mayor",
                Address = "Avenida Parque",
                PhoneNumber = "3200000000",
                Email = "parque@yopmail.com",
                CityId = 3,
                HeadquarterMedicines =
               [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = advil! },
                    new HeadquarterMedicine { Medicine = electrolit! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                ]
            };

            var mall = new Headquarter
            {
                Name = "Mall",
                Address = "Mall principal",
                PhoneNumber = "3300000000",
                Email = "mall@yopmail.com",
                CityId = 2,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = naproxeno! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = advil! },
                ]
            };

            var coffe = new Headquarter
            {
                Name = "Café",
                Address = "Avenida Café",
                PhoneNumber = "3400000000",
                Email = "cafe@yopmail.com",
                CityId = 4,
                HeadquarterMedicines =
               [
                   new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = sevedol! },
                    new HeadquarterMedicine { Medicine = electrolit! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = minoxidil! },
                ]
            };

            var building = new Headquarter
            {
                Name = "Edificio principal",
                Address = "Avenida principal",
                PhoneNumber = "3180000000",
                Email = "edificio@yopmail.com",
                CityId = 5,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = ibuprofeno! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = advil! },
                ]
            };

            var river = new Headquarter
            {
                Name = "Río",
                Address = "Avenida del río",
                PhoneNumber = "3160000000",
                Email = "rio@yopmail.com",
                CityId = 1,
                HeadquarterMedicines =
               [
                    new HeadquarterMedicine { Medicine = electrolit! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = minoxidil! },
                ]
            };

            var church = new Headquarter
            {
                Name = "Iglesia",
                Address = "Avenida iglesia",
                PhoneNumber = "3140000000",
                Email = "iglesia@yopmail.com",
                CityId = 7,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = ibuprofeno! },
                    new HeadquarterMedicine { Medicine = naproxeno! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                ]
            };

            var hospital = new Headquarter
            {
                Name = "Zona hospitalaria",
                Address = "Avenida Hospital",
                PhoneNumber = "3120000000",
                Email = "hospital@yopmail.com",
                CityId = 6,
                HeadquarterMedicines =
               [
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = sevedol! },
                    new HeadquarterMedicine { Medicine = electrolit! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = minoxidil! },
                ]
            };

            var street = new Headquarter
            {
                Name = "Calle principal",
                Address = "Avenida nueva",
                PhoneNumber = "320000000",
                Email = "calle@yopmail.com",
                CityId = 1,
                HeadquarterMedicines =
                [
                    new HeadquarterMedicine { Medicine = acetaminofen! },
                    new HeadquarterMedicine { Medicine = dolex! },
                    new HeadquarterMedicine { Medicine = ibuprofeno! },
                    new HeadquarterMedicine { Medicine = naproxeno! },
                    new HeadquarterMedicine { Medicine = tramadol! },
                    new HeadquarterMedicine { Medicine = loratadina! },
                    new HeadquarterMedicine { Medicine = amoxicilina! },
                    new HeadquarterMedicine { Medicine = buscapina! },
                    new HeadquarterMedicine { Medicine = advil! },
                ]
            };

            _context.Headquarters.Add(center);
            _context.Headquarters.Add(pub);
            _context.Headquarters.Add(park);
            _context.Headquarters.Add(mall);
            _context.Headquarters.Add(coffe);
            _context.Headquarters.Add(building);
            _context.Headquarters.Add(river);
            _context.Headquarters.Add(church);
            _context.Headquarters.Add(hospital);
            _context.Headquarters.Add(street);
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckRatingsAsync()
    {
        if (!_context.Ratings.Any())
        {
            var ratingsSQLScript = File.ReadAllText("Data\\Ratings.sql");
            await _context.Database.ExecuteSqlRawAsync(ratingsSQLScript);
        }
    }

    private async Task CheckModulesAsync()
    {
        if (!_context.Modules.Any())
        {
            var modulesSQLScript = File.ReadAllText("Data\\Modules.sql");
            await _context.Database.ExecuteSqlRawAsync(modulesSQLScript);
        }
    }

    private async Task CheckFlagsCountriesAsync()
    {
        foreach (var country in _context.Countries)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{country.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "countries");
            }

            country.Image = imagePath;
            _context.Entry(country).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckFlagsStatesAsync()
    {
        foreach (var state in _context.States)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\States\\{state.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "states");
            }

            state.Image = imagePath;
            _context.Entry(state).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckImagesMedicineAsync()
    {
        foreach (var medicine in _context.MedicinesStock)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Medicines\\{medicine.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "medicines");
            }

            medicine.Image = imagePath;
            _context.Entry(medicine).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckImagesCitiesAsync()
    {
        foreach (var cities in _context.Cities)
        {
            var imagePath = string.Empty;
            var filePath = $"{Environment.CurrentDirectory}\\Images\\Cities\\{cities.Name}.png";
            if (File.Exists(filePath))
            {
                var fileBytes = File.ReadAllBytes(filePath);
                imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "cities");
            }

            cities.Image = imagePath;
            _context.Entry(cities).Property(c => c.Image).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Adviser.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

            var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            await _usersUnitOfWork.ConfirmEmailAsync(user, token);
        }

        return user;
    }

    private async Task CheckPQRsAsync()
    {
        if (!_context.PQRs.Any())
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == "usuario@yopmail.com");

            var pqr1 = new PQR
            {
                Code = "PQR-0001",
                Description = "Problema con el medicamento 1",
                User = user!,
            };
            _context.Add(pqr1);

            var pqr2 = new PQR
            {
                Code = "PQR-0002",
                Description = "Problema con el medicamento 2",
                User = user!,
            };
            _context.Add(pqr2);

            var pqr3 = new PQR
            {
                Code = "PQR-0003",
                Description = "Problema con el medicamento 3",
                User = user!,
            };
            _context.Add(pqr3);

            var pqr4 = new PQR
            {
                Code = "PQR-0004",
                Description = "Problema con el medicamento 4",
                User = user!,
            };
            _context.Add(pqr4);

            var pqr5 = new PQR
            {
                Code = "PQR-0005",
                Description = "Problema con el medicamento 5",
                User = user!,
            };
            _context.Add(pqr5);

            var pqr6 = new PQR
            {
                Code = "PQR-0006",
                Description = "Problema con el medicamento 6",
                User = user!,
            };
            _context.Add(pqr6);

            var pqr7 = new PQR
            {
                Code = "PQR-0007",
                Description = "Problema con el medicamento 7",
                User = user!,
            };
            _context.Add(pqr7);

            var pqr8 = new PQR
            {
                Code = "PQR-0008",
                Description = "Problema con el medicamento 8",
                User = user!,
            };
            _context.Add(pqr8);

            var pqr9 = new PQR
            {
                Code = "PQR-0009",
                Description = "Problema con el medicamento 9",
                User = user!,
            };
            _context.Add(pqr9);

            var pqr10 = new PQR
            {
                Code = "PQR-0010",
                Description = "Problema con el medicamento 10",
                User = user!,
            };
            _context.Add(pqr10);

            await _context.SaveChangesAsync();
        }
    }
}