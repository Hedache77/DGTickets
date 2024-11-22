using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface IPQRsUnitOfWork
{
    Task<IEnumerable<PQR>> GetComboAsync(string userId);

    Task<IEnumerable<PQR>> GetComboAsync();

    Task<ActionResponse<PQR>> AddAsync(PQRDTO pqrDTO);

    Task<ActionResponse<PQR>> UpdateAsync(PQRDTO pqrDTO);

    Task<ActionResponse<PQR>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<PQR>>> GetAsync();

    Task<ActionResponse<IEnumerable<PQR>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}