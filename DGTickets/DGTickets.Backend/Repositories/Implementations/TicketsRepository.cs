using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class TicketsRepository : GenericRepository<Ticket>, ITicketsRepository
{
    private readonly DataContext _context;
    private readonly IUsersRepository _usersRepository;
    private readonly IMailHelper _mailHelper;

    public TicketsRepository(DataContext context, IUsersRepository usersRepository, IMailHelper mailHelper) : base(context)
    {
        _context = context;
        _usersRepository = usersRepository;
        _mailHelper = mailHelper;
    }

    public override async Task<ActionResponse<IEnumerable<Ticket>>> GetAsync()
    {
        var tickets = await _context.Tickets
            .Include(x => x.Headquarter)
            .Include(x => x.User)
            .Include(x => x.TicketMedicines!)
            .ThenInclude(x => x.Medicine)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Ticket>>
        {
            WasSuccess = true,
            Result = tickets
        };
    }

    public override async Task<ActionResponse<Ticket>> GetAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(x => x.Headquarter)
            .Include(x => x.User)
            .Include(x => x.TicketMedicines!)
            .ThenInclude(x => x.Medicine)
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

    public override async Task<ActionResponse<IEnumerable<Ticket>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Tickets
           .Include(x => x.Headquarter)
           .Include(x => x.User)
           .Include(x => x.TicketMedicines)
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Ticket>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderByDescending(x => x.OrderDate)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<Ticket>> AddAsync(TicketDTO ticketDTO)
    {
        var headquarter = await _context.Headquarters.FindAsync(ticketDTO.HeadquarterId);
        if (headquarter == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR006"
            };
        }

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
            Headquarter = headquarter,
            TicketType = ticketDTO.TicketType,
            UserId = user!.Id,
            OrderDate = DateTime.Now,
            ServiceDate = DateTime.Now,
            TicketMedicines = new List<TicketMedicine>()
        };

        string emailHtmlEN = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Document</title>
        </head>
        <body>
            <header style=""width: 100%; height: 100px; background-color: #207870; color: white; font-size: 50px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center;"">
                DGTICKETS
            </header>
            <section style=""width: 100%; height: 200px; background-color: #f8f9f9; color: black; font-size: 20px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center; margin-top: 50px;"">
                <h1>THANK YOU FOR YOUR TICKET</h1>
                <h2>YOUR ORDER HAS BEEN CONFIRMED</h2>
                <h6>YOUR TICKET NUMBER IS: {code}</h6>
            </section>
        </body>
        </html>";

        string subjectHtmlEN = "📣 ¡Your ticket has been processed successfully!📣";
        string subjectHtmlES = "📣 ¡Su turno ha sido creado con éxito!📣";

        string emailHtmlES = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Document</title>
        </head>
        <body>
            <header style=""width: 100%; height: 100px; background-color: #207870; color: white; font-size: 50px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center;"">
                DGTICKETS
            </header>
            <section style=""width: 100%; height: 200px; background-color: #f8f9f9; color: black; font-size: 20px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center; margin-top: 50px;"">
                <h1>GRACIAS POR SOLICITAR SU TURNO</h1>
                <h2>SU TURNO HA SIDO CONFIRMADO</h2>
                <h6>SU NÚMERO DE TICKET ES: {code}</h6>
            </section>
        </body>
        </html>";

        _context.Add(ticket);
        try
        {
            await _context.SaveChangesAsync();
            if (ticketDTO.Language == "es")
            {
                _mailHelper.SendMail(user.FullName, user.Email!, subjectHtmlES, emailHtmlES, ticketDTO.Language);
            }
            else
            {
                _mailHelper.SendMail(user.FullName, user.Email!, subjectHtmlEN, emailHtmlEN, ticketDTO.Language!);
            }
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

    public async Task<IEnumerable<Ticket>> GetComboAsync(int headquarterId)
    {
        return await _context.Tickets
            .Where(x => x.HeadquarterId == headquarterId)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetComboAsync()
    {
        return await _context.Tickets
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
    }

    public async Task<ActionResponse<Ticket>> UpdateAsync(TicketDTO ticketDTO)
    {
        var currentTicket = await _context.Tickets.FindAsync(ticketDTO.Id);
        if (currentTicket == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR016"
            };
        }

        var headquarter = await _context.Headquarters.FindAsync(ticketDTO.HeadquarterId);
        if (headquarter == null)
        {
            return new ActionResponse<Ticket>
            {
                WasSuccess = false,
                Message = "ERR006"
            };
        }

        currentTicket.Headquarter = headquarter;
        currentTicket.TicketType = ticketDTO.TicketType;

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

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Tickets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
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