using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class CitiesRepository : GenericRepository<City>, ICitiesRepository
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public CitiesRepository(DataContext context, IFileStorage fileStorage) : base(context)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync()
    {
        var cities = await _context.Cities
            .Include(x => x.State)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ActionResponse<IEnumerable<City>>
        {
            WasSuccess = true,
            Result = cities
        };
    }

    public override async Task<ActionResponse<City>> GetAsync(int id)
    {
        var city = await _context.Cities
         .Include(x => x.State)
         .FirstOrDefaultAsync(c => c.Id == id);

        if (city == null)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<City>
        {
            WasSuccess = true,
            Result = city
        };
    }

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Cities
            .Include(x => x.State)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.State!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<City>>
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
        var queryable = _context.Cities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.State!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<City>> AddAsync(CityDTO cityDTO)
    {
        var state = await _context.States.FindAsync(cityDTO.StateId);
        if (state == null)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR008"
            };
        }

        var city = new City
        {
            State = state,
            Name = cityDTO.Name,
        };

        if (!string.IsNullOrEmpty(cityDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(cityDTO.Image!);
            city.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "cities");
        }

        _context.Add(city);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<City>
            {
                WasSuccess = true,
                Result = city
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<City>> GetComboAsync(int stateId)
    {
        return await _context.Cities
        .Where(x => x.StateId == stateId)
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetComboAsync()
    {
        return await _context.Cities
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<ActionResponse<City>> UpdateAsync(CityDTO cityDTO)
    {
        var currentCity = await _context.Cities.FindAsync(cityDTO.Id);
        if (currentCity == null)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR005"
            };
        }

        var state = await _context.States.FindAsync(cityDTO.StateId);
        if (state == null)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR008"
            };
        }

        if (!string.IsNullOrEmpty(cityDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(cityDTO.Image!);
            currentCity.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "cities");
        }

        currentCity.State = state;
        currentCity.Name = cityDTO.Name;

        _context.Update(currentCity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<City>
            {
                WasSuccess = true,
                Result = currentCity
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<City>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}