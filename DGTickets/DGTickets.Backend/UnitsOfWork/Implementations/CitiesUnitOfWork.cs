using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class CitiesUnitOfWork : GenericUnitOfWork<City>, ICitiesUnitOfWork
{
    private readonly ICitiesRepository _citiesRepository;

    public CitiesUnitOfWork(IGenericRepository<City> repository, ICitiesRepository citiesRepository) : base(repository)
    {
        _citiesRepository = citiesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync() => await _citiesRepository.GetAsync();

    public override async Task<ActionResponse<City>> GetAsync(int id) => await _citiesRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination) => await _citiesRepository.GetAsync(pagination);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _citiesRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<City>> AddAsync(CityDTO cityDTO) => await _citiesRepository.AddAsync(cityDTO);

    public async Task<IEnumerable<City>> GetComboAsync(int stateId) => await _citiesRepository.GetComboAsync(stateId);

    public async Task<ActionResponse<City>> UpdateAsync(CityDTO cityDTO) => await _citiesRepository.UpdateAsync(cityDTO);

    public async Task<IEnumerable<City>> GetComboAsync() => await _citiesRepository.GetComboAsync();
}