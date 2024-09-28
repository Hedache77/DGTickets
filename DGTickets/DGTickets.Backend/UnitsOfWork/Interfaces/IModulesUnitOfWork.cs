using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface IModulesUnitOfWork
{
    Task<IEnumerable<Module>> GetComboAsync();

    Task<IEnumerable<Module>> GetComboAsync(int headquarterId);

    Task<ActionResponse<Module>> AddAsync(ModuleDTO moduleDTO);

    Task<ActionResponse<Module>> UpdateAsync(ModuleDTO moduleDTO);

    Task<ActionResponse<Module>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Module>>> GetAsync();

    Task<ActionResponse<IEnumerable<Module>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}