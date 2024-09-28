using DGTickets.Backend.Repositories.Implementations;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class HeadquartersUnitOfWork : GenericUnitOfWork<Headquarter>, IHeadquartersUnitOfWork
{
    private readonly IHeadquartersRepository _headquartersRepository;

    public HeadquartersUnitOfWork(IGenericRepository<Headquarter> repository, IHeadquartersRepository headquartersRepository) : base(repository)
    {
        _headquartersRepository = headquartersRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync() => await _headquartersRepository.GetAsync();

    public override async Task<ActionResponse<Headquarter>> GetAsync(int id) => await _headquartersRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync(PaginationDTO pagination) => await _headquartersRepository.GetAsync(pagination);

    public async Task<ActionResponse<Headquarter>> AddAsync(HeadquarterDTO headquarterDTO) => await _headquartersRepository.AddAsync(headquarterDTO);

    public async Task<IEnumerable<Headquarter>> GetComboAsync(int cityId) => await _headquartersRepository.GetComboAsync(cityId);

    public async Task<IEnumerable<Headquarter>> GetComboAsync() => await _headquartersRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _headquartersRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<Headquarter>> UpdateAsync(HeadquarterDTO headquarterDTO) => await _headquartersRepository.UpdateAsync(headquarterDTO);
}