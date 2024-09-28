using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class RatingsUnitOfWork : GenericUnitOfWork<Rating>, IRatingsUnitOfWork
{
    private readonly IRatingsRepository _ratingsRepository;

    public RatingsUnitOfWork(IGenericRepository<Rating> repository, IRatingsRepository ratingsRepository) : base(repository)
    {
        _ratingsRepository = ratingsRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Rating>>> GetAsync() => await _ratingsRepository.GetAsync();

    public override async Task<ActionResponse<Rating>> GetAsync(int id) => await _ratingsRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Rating>>> GetAsync(PaginationDTO pagination) => await _ratingsRepository.GetAsync(pagination);

    public async Task<IEnumerable<Rating>> GetComboAsync() => await _ratingsRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _ratingsRepository.GetTotalRecordsAsync(pagination);
}