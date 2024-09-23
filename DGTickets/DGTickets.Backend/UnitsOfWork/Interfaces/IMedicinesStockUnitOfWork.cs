using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface IMedicinesStockUnitOfWork
{
    Task<ActionResponse<MedicineStock>> AddAsync(MedicineStock medicineStock);

    Task<ActionResponse<MedicineStock>> UpdateAsync(MedicineStock medicineStock);

    Task<ActionResponse<IEnumerable<MedicineStock>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}