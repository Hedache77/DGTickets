using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Implementations;

public class MedicinesStockUnitOfWork : GenericUnitOfWork<MedicineStock>, IMedicinesStockUnitOfWork
{
    private readonly IMedicinesStockRepository _medicinesStockRepository;

    public MedicinesStockUnitOfWork(IGenericRepository<MedicineStock> repository, IMedicinesStockRepository medicinesStockRepository) : base(repository)
    {
        _medicinesStockRepository = medicinesStockRepository;
    }

    public override async Task<ActionResponse<MedicineStock>> AddAsync(MedicineStock medicineStock) => await _medicinesStockRepository.AddAsync(medicineStock);

    public override async Task<ActionResponse<MedicineStock>> UpdateAsync(MedicineStock medicineStock) => await _medicinesStockRepository.UpdateAsync(medicineStock);

    public override async Task<ActionResponse<IEnumerable<MedicineStock>>> GetAsync(PaginationDTO pagination) => await _medicinesStockRepository.GetAsync(pagination);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _medicinesStockRepository.GetTotalRecordsAsync(pagination);

    public async Task<IEnumerable<MedicineStock>> GetComboAsync() => await _medicinesStockRepository.GetComboAsync();
}