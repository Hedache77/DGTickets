using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class TicketMedicinesUnitOfWork : GenericUnitOfWork<TicketMedicine>, ITicketMedicinesUnitOfWork
{
    private readonly ITicketMedicinesRepository _ticketMedicinesRepository;

    public TicketMedicinesUnitOfWork(IGenericRepository<TicketMedicine> repository, ITicketMedicinesRepository ticketMedicinesRepository) : base(repository)
    {
        _ticketMedicinesRepository = ticketMedicinesRepository;
    }

    public async Task<ActionResponse<TicketMedicine>> AddAsync(TicketMedicineDTO ticketMedicineDTO) => await _ticketMedicinesRepository.AddAsync(ticketMedicineDTO);

    public async Task<IEnumerable<TicketMedicine>> GetComboAsync(int ticketId) => await _ticketMedicinesRepository.GetComboAsync(ticketId);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _ticketMedicinesRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<TicketMedicine>>> GetAsync(PaginationDTO pagination) => await _ticketMedicinesRepository.GetAsync(pagination);
}