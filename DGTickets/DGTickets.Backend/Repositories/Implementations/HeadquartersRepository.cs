using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class HeadquartersRepository : GenericRepository<Headquarter>, IHeadquartersRepository
{
    private readonly DataContext _context;

    public HeadquartersRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync()
    {
        var headquarters = await _context.Headquarters
            .Include(x => x.City)
            .Include(x => x.HeadquarterMedicines!)
            .ThenInclude(x => x.Medicine)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Headquarter>>
        {
            WasSuccess = true,
            Result = headquarters
        };
    }

    public override async Task<ActionResponse<Headquarter>> GetAsync(int id)
    {
        var headquarter = await _context.Headquarters
             .Include(x => x.City)
             .Include(x => x.HeadquarterMedicines!)
             .ThenInclude(x => x.Medicine)
             .FirstOrDefaultAsync(c => c.Id == id);

        if (headquarter == null)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Headquarter>
        {
            WasSuccess = true,
            Result = headquarter
        };
    }

    public override async Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Headquarters
            .Include(x => x.City)
            .Include(x => x.HeadquarterMedicines)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Headquarter>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<Headquarter>> AddAsync(HeadquarterDTO headquarterDTO)
    {
        var city = await _context.Cities.FindAsync(headquarterDTO.CityId);
        if (city == null)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR005"
            };
        }

        var headquarter = new Headquarter
        {
            City = city,
            Name = headquarterDTO.Name,
            Address = headquarterDTO.Address,
            PhoneNumber = headquarterDTO.PhoneNumber,
            Email = headquarterDTO.Email,
            HeadquarterMedicines = new List<HeadquarterMedicine>()
        };

        _context.Add(headquarter);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Headquarter>
            {
                WasSuccess = true,
                Result = headquarter
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<Headquarter>> GetComboAsync(int cityId)
    {
        return await _context.Headquarters
            .Where(x => x.CityId == cityId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Headquarter>> GetComboAsync()
    {
        return await _context.Headquarters
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<ActionResponse<Headquarter>> UpdateAsync(HeadquarterDTO headquarterDTO)
    {
        var currentHeadquarter = await _context.Headquarters.FindAsync(headquarterDTO.Id);
        if (currentHeadquarter == null)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR006"
            };
        }

        var city = await _context.Cities.FindAsync(headquarterDTO.CityId);
        if (city == null)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR005"
            };
        }

        currentHeadquarter.City = city;
        currentHeadquarter.Name = headquarterDTO.Name;
        currentHeadquarter.Address = headquarterDTO.Address;
        currentHeadquarter.PhoneNumber = headquarterDTO.PhoneNumber;
        currentHeadquarter.Email = headquarterDTO.Email;

        _context.Update(currentHeadquarter);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Headquarter>
            {
                WasSuccess = true,
                Result = currentHeadquarter
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Headquarter>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Headquarters.AsQueryable();

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