using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using System.Text.RegularExpressions;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class TicketsUnitOfWork : GenericUnitOfWork<Ticket>, ITicketsUnitOfWork
{
    private readonly ITicketsRepository _ticketsRepository;

    public TicketsUnitOfWork(IGenericRepository<Ticket> repository, ITicketsRepository ticketsRepository) : base(repository)
    {
        _ticketsRepository = ticketsRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Ticket>>> GetAsync(PaginationDTO pagination) => await _ticketsRepository.GetAsync(pagination);

    public override async Task<ActionResponse<Ticket>> GetAsync(int id) => await _ticketsRepository.GetAsync(id);

    public async Task<ActionResponse<Ticket>> AddAsync(TicketDTO ticketDTO) => await _ticketsRepository.AddAsync(ticketDTO);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _ticketsRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<Ticket>> UpdateAsync(TicketDTO ticketDTO) => await _ticketsRepository.UpdateAsync(ticketDTO);
}