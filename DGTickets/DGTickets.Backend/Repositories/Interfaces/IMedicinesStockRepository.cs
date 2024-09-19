using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.Repositories.Interfaces;

public interface IMedicinesStockRepository
{
    Task<ActionResponse<MedicineStock>> AddAsync(MedicineStock medicineStock);

    Task<ActionResponse<MedicineStock>> UpdateAsync(MedicineStock medicineStock);
}