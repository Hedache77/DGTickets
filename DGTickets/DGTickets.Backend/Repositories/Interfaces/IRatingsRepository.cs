using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.Repositories.Interfaces;

public interface IRatingsRepository
{
    Task<ActionResponse<Rating>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Rating>>> GetAsync();

    Task<IEnumerable<Rating>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Rating>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}