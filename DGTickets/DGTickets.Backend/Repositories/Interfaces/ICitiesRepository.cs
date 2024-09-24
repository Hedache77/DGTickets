using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.Repositories.Interfaces;

public interface ICitiesRepository
{
    Task<IEnumerable<City>> GetComboAsync(int stateId);

    Task<ActionResponse<City>> AddAsync(CityDTO cityDTO);

    Task<ActionResponse<City>> UpdateAsync(CityDTO cityDTO);

    Task<ActionResponse<City>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<City>>> GetAsync();

    Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}