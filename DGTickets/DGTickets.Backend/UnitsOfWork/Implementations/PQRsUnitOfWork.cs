using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class PQRsUnitOfWork : GenericUnitOfWork<PQR>, IPQRsUnitOfWork
{
    private readonly IPQRsRepository _pqrRepository;

    public PQRsUnitOfWork(IGenericRepository<PQR> repository, IPQRsRepository pqrRepository) : base(repository)
    {
        _pqrRepository = pqrRepository;
    }

    public override async Task<ActionResponse<PQR>> GetAsync(int id) => await _pqrRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<PQR>>> GetAsync() => await _pqrRepository.GetAsync();

    public override async Task<ActionResponse<IEnumerable<PQR>>> GetAsync(PaginationDTO pagination) => await _pqrRepository.GetAsync(pagination);

    public async Task<ActionResponse<PQR>> AddAsync(PQRDTO pqrDTO) => await _pqrRepository.AddAsync(pqrDTO);

    public async Task<IEnumerable<PQR>> GetComboAsync() => await _pqrRepository.GetComboAsync();

    public async Task<IEnumerable<PQR>> GetComboAsync(string userId) => await _pqrRepository.GetComboAsync(userId);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _pqrRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<PQR>> UpdateAsync(PQRDTO pqrDTO) => await _pqrRepository.UpdateAsync(pqrDTO);
}