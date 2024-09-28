using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class StatesRepository : GenericRepository<State>, IStatesRepository
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public StatesRepository(DataContext context, IFileStorage fileStorage) : base(context)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
    {
        var states = await _context.States
            .Include(x => x.Country)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ActionResponse<IEnumerable<State>>
        {
            WasSuccess = true,
            Result = states
        };
    }

    public override async Task<ActionResponse<State>> GetAsync(int id)
    {
        var state = await _context.States
         .Include(x => x.Country)
         .FirstOrDefaultAsync(c => c.Id == id);

        if (state == null)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<State>
        {
            WasSuccess = true,
            Result = state
        };
    }

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.States
            .Include(x => x.Country)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Country!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<State>>
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
        var queryable = _context.States.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Country!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<State>> AddAsync(StateDTO stateDTO)
    {
        var country = await _context.Countries.FindAsync(stateDTO.CountryId);
        if (country == null)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR009"
            };
        }

        var state = new State
        {
            Country = country,
            Name = stateDTO.Name,
        };

        if (!string.IsNullOrEmpty(stateDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(stateDTO.Image!);
            state.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "teams");
        }

        _context.Add(state);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<State>
            {
                WasSuccess = true,
                Result = state
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<State>> GetComboAsync(int countryId)
    {
        return await _context.States
        .Where(x => x.CountryId == countryId)
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<IEnumerable<State>> GetComboAsync()
    {
        return await _context.States
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<ActionResponse<State>> UpdateAsync(StateDTO stateDTO)
    {
        var currentState = await _context.States.FindAsync(stateDTO.Id);
        if (currentState == null)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR005"
            };
        }

        var country = await _context.Countries.FindAsync(stateDTO.CountryId);
        if (country == null)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        if (!string.IsNullOrEmpty(stateDTO.Image))
        {
            var imageBase64 = Convert.FromBase64String(stateDTO.Image!);
            currentState.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "teams");
        }

        currentState.Country = country;
        currentState.Name = stateDTO.Name;

        _context.Update(currentState);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<State>
            {
                WasSuccess = true,
                Result = currentState
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<State>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}