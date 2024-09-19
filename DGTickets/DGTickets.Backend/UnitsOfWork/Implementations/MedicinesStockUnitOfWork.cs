using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Interfaces;
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
}