using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface ITicketMedicinesUnitOfWork
{
    Task<IEnumerable<TicketMedicine>> GetComboAsync(int ticketId);

    Task<ActionResponse<TicketMedicine>> AddAsync(TicketMedicineDTO ticketMedicineDTO);

    Task<ActionResponse<IEnumerable<TicketMedicine>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}