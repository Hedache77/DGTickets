using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class TicketMedicinesRepository : GenericRepository<TicketMedicine>, ITicketMedicinesRepository
{
    private readonly DataContext _context;

    public TicketMedicinesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ActionResponse<TicketMedicine>> AddAsync(TicketMedicineDTO ticketMedicineDTO)
    {
        var ticket = await _context.Tickets.FindAsync(ticketMedicineDTO.TicketId);
        if (ticket == null)
        {
            return new ActionResponse<TicketMedicine>
            {
                WasSuccess = false,
                Message = "ERR016"
            };
        }

        var medicine = await _context.MedicinesStock.FindAsync(ticketMedicineDTO.MedicineId);
        if (medicine == null)
        {
            return new ActionResponse<TicketMedicine>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        var ticketMedicine = new TicketMedicine
        {
            Ticket = ticket,
            Medicine = medicine,
        };

        _context.Add(ticketMedicine);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<TicketMedicine>
            {
                WasSuccess = true,
                Result = ticketMedicine
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<TicketMedicine>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<TicketMedicine>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<TicketMedicine>> GetComboAsync(int ticketId)
    {
        return await _context.TicketMedicines
            .Include(x => x.Medicine)
            .Where(x => x.TicketId == ticketId)
            .OrderBy(x => x.Medicine.Name)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.TicketMedicines.AsQueryable();
        queryable = queryable.Where(x => x.TicketId == pagination.Id);

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Medicine.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public override async Task<ActionResponse<IEnumerable<TicketMedicine>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.TicketMedicines
            .Include(x => x.Medicine)
            .AsQueryable();
        queryable = queryable.Where(x => x.TicketId == pagination.Id);

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Medicine.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<TicketMedicine>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Medicine.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
}