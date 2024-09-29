using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public CountriesRepository(DataContext context, IFileStorage fileStorage) : base(context)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public override async Task<ActionResponse<Country>> AddAsync(Country country)
    {
        var countryNew = new Country
        {
            Name = country.Name,
        };

        if (!string.IsNullOrEmpty(country.Image))
        {
            var imageBase64 = Convert.FromBase64String(country.Image!);
            countryNew.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "countries");
        }

        _context.Add(countryNew);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = countryNew
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public override async Task<ActionResponse<Country>> UpdateAsync(Country country)
    {
        var currentCountry = await _context.Countries.FindAsync(country.Id);
        if (currentCountry == null)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        if (!string.IsNullOrEmpty(country.Image))
        {
            var imageBase64 = Convert.FromBase64String(country.Image!);
            currentCountry.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "countries");
        }

        currentCountry.Name = country.Name;

        _context.Update(currentCountry);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = currentCountry
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public override async Task<ActionResponse<Country>> GetAsync(int id)
    {
        var country = await _context.Countries
             //.Include(x => x.Teams)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (country == null)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Country>
        {
            WasSuccess = true,
            Result = country
        };
    }

    public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
    {
        var countries = await _context.Countries
            //.Include(x => x.Teams)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Country>>
        {
            WasSuccess = true,
            Result = countries
        };
    }

    public async Task<IEnumerable<Country>> GetComboAsync()
    {
        return await _context.Countries
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Countries
            //.Include(x => x.Teams)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Country>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Countries.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}