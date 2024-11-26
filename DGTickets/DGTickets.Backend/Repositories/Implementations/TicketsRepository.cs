using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DGTickets.Backend.Repositories.Implementations;

public class TicketsRepository : GenericRepository<Ticket>, ITicketsRepository
{
    private readonly DataContext _context;
    private readonly IUsersRepository _usersRepository;

    public TicketsRepository(DataContext context, IUsersRepository usersRepository) : base(context)
    {
        _context = context;
        _usersRepository = usersRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Ticket>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Tickets;

        return new ActionResponse<IEnumerable<Ticket>>
        {
            WasSuccess = true,
            Result = await queryable
                .Include(x => x.Headquarter)
                .Include(x => x.User)
                .OrderBy(x => x.Code)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public override async Task<ActionResponse<Ticket>> GetAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(x => x.User!)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (ticket == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Ticket>
        {
            WasSuccess = true,
            Result = ticket
        };
    }

    //public override async Task<ActionResponse<Ticket>> GetAsync(int id)
    //{
    //    var ticket = await _context.Tickets
    //        .Include(x => x.User!)
    //        .FirstOrDefaultAsync(c => c.Id == id);

    //    if (ticket == null)
    //    {
    //        return new ActionResponse<Ticket>
    //        {
    //            WasSuccess = false,
    //            Message = "ERR001"
    //        };
    //    }

    //    return new ActionResponse<Ticket>
    //    {
    //        WasSuccess = true,
    //        Result = ticket
    //    };
    //}

    public async Task<ActionResponse<Ticket>> AddAsync(TicketDTO ticketDTO)
    {
        var code = string.Empty;
        var exists = true;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == ticketDTO.User);
        do
        {
            code = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            var currentCode = await _context.Tickets.FirstOrDefaultAsync(x => x.Code == code);
            exists = currentCode != null;
        } while (exists);

        var ticket = new Ticket
        {
            Code = code,
            HeadquarterId = ticketDTO.HeadquarterId,
            TicketType = ticketDTO.TicketType,
            UserId = user.Id,
            OrderDate = DateTime.Now,
            ServiceDate = DateTime.Now
        };

        _context.Add(ticket);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Ticket>
            {
                WasSuccess = true,
                Result = ticket
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Tickets.AsQueryable();

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<Ticket>> UpdateAsync(TicketDTO ticketDTO)
    {
        var currentTicket = await _context.Tickets.FindAsync(ticketDTO.Id);
        if (currentTicket == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR014"
            };
        }

        _context.Update(currentTicket);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Ticket>
            {
                WasSuccess = true,
                Result = currentTicket
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<Ticket>> GetAsync(string code)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Code == code);

        if (ticket == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Ticket>
        {
            WasSuccess = true,
            Result = ticket
        };
    }
}