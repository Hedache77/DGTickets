using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class PQRsRepository : GenericRepository<PQR>, IPQRsRepository
{
    private readonly DataContext _context;
    private readonly IUsersRepository _usersRepository;

    public PQRsRepository(DataContext context, IUsersRepository usersRepository) : base(context)
    {
        _context = context;
        _usersRepository = usersRepository;
    }

    public override async Task<ActionResponse<IEnumerable<PQR>>> GetAsync()
    {
        var pqrs = await _context.PQRs
            .Include(x => x.User)
            .OrderBy(x => x.Code)
            .ToListAsync();
        return new ActionResponse<IEnumerable<PQR>>
        {
            WasSuccess = true,
            Result = pqrs
        };
    }

    public override async Task<ActionResponse<PQR>> GetAsync(int id)
    {
        var pqr = await _context.PQRs
         .Include(x => x.User)
         .FirstOrDefaultAsync(c => c.Id == id);

        if (pqr == null)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<PQR>
        {
            WasSuccess = true,
            Result = pqr
        };
    }

    public override async Task<ActionResponse<IEnumerable<PQR>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.PQRs
            .Include(x => x.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code!.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<PQR>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Code)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.PQRs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code!.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<PQR>> AddAsync(PQRDTO pqrDTO)
    {
        var user = await _usersRepository.GetUserAsync(pqrDTO.UserId);
        if (user == null)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = "ERR015"
            };
        }

        var code = string.Empty;
        var exists = true;
        do
        {
            code = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            var currentPQR = await _context.PQRs.FirstOrDefaultAsync(x => x.Code == code);
            exists = currentPQR != null;
        } while (exists);

        var pqr = new PQR
        {
            Code = code,
            Description = pqrDTO.Description,
            User = user
        };

        _context.Add(pqr);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<PQR>
            {
                WasSuccess = true,
                Result = pqr
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<PQR>> GetComboAsync(string userId)
    {
        return await _context.PQRs
        .Where(x => x.UserId == userId)
        .OrderBy(x => x.Code)
        .ToListAsync();
    }

    public async Task<IEnumerable<PQR>> GetComboAsync()
    {
        return await _context.PQRs
        .OrderBy(x => x.Code)
        .ToListAsync();
    }

    public async Task<ActionResponse<PQR>> UpdateAsync(PQRDTO pqrDTO)
    {
        var currentPQR = await _context.PQRs.FindAsync(pqrDTO.Id);
        if (currentPQR == null)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = "ERR014"
            };
        }

        currentPQR.Code = pqrDTO.Code;
        currentPQR.Description = pqrDTO.Description;
        currentPQR.UserId = pqrDTO.UserId;

        _context.Update(currentPQR);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<PQR>
            {
                WasSuccess = true,
                Result = currentPQR
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<PQR>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}