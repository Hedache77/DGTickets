using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface IHeadquarterMedicinesUnitOfWork
{
    Task<IEnumerable<HeadquarterMedicine>> GetComboAsync(int headquarterId);

    Task<ActionResponse<HeadquarterMedicine>> AddAsync(HeadquarterMedicineDTO headquarterMedicineDTO);

    Task<ActionResponse<IEnumerable<HeadquarterMedicine>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}