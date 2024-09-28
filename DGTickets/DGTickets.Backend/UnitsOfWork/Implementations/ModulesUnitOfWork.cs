using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class ModulesUnitOfWork : GenericUnitOfWork<Module>, IModulesUnitOfWork
{
    private readonly IModulesRepository _modulesRepository;

    public ModulesUnitOfWork(IGenericRepository<Module> repository, IModulesRepository modulesRepository) : base(repository)
    {
        _modulesRepository = modulesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Module>>> GetAsync() => await _modulesRepository.GetAsync();

    public override async Task<ActionResponse<Module>> GetAsync(int id) => await _modulesRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Module>>> GetAsync(PaginationDTO pagination) => await _modulesRepository.GetAsync(pagination);

    public async Task<ActionResponse<Module>> AddAsync(ModuleDTO moduleDTO) => await _modulesRepository.AddAsync(moduleDTO);

    public async Task<IEnumerable<Module>> GetComboAsync(int headquarterId) => await _modulesRepository.GetComboAsync(headquarterId);

    public async Task<IEnumerable<Module>> GetComboAsync() => await _modulesRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _modulesRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<Module>> UpdateAsync(ModuleDTO moduleDTO) => await _modulesRepository.UpdateAsync(moduleDTO);
}