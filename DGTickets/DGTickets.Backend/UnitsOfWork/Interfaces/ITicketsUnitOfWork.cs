using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface ITicketsUnitOfWork
{
    Task<ActionResponse<Ticket>> AddAsync(TicketDTO ticketDTO);

    Task<ActionResponse<Ticket>> UpdateAsync(TicketDTO ticketDTO);

    Task<ActionResponse<Ticket>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Ticket>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}