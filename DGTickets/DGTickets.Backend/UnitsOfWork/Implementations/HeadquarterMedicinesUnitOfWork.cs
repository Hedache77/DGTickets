using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class HeadquarterMedicinesUnitOfWork : GenericUnitOfWork<HeadquarterMedicine>, IHeadquarterMedicinesUnitOfWork
{
    private readonly IHeadquarterMedicinesRepository _headquarterMedicinesRepository;

    public HeadquarterMedicinesUnitOfWork(IGenericRepository<HeadquarterMedicine> repository, IHeadquarterMedicinesRepository headquarterMedicinesRepository) : base(repository)
    {
        _headquarterMedicinesRepository = headquarterMedicinesRepository;
    }

    public async Task<ActionResponse<HeadquarterMedicine>> AddAsync(HeadquarterMedicineDTO headquarterMedicineDTO) => await _headquarterMedicinesRepository.AddAsync(headquarterMedicineDTO);

    public async Task<IEnumerable<HeadquarterMedicine>> GetComboAsync(int headquarterId) => await _headquarterMedicinesRepository.GetComboAsync(headquarterId);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _headquarterMedicinesRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<HeadquarterMedicine>>> GetAsync(PaginationDTO pagination) => await _headquarterMedicinesRepository.GetAsync(pagination);
}