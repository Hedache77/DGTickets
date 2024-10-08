﻿using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.Repositories.Interfaces;

public interface IHeadquartersRepository
{
    Task<IEnumerable<Headquarter>> GetComboAsync();

    Task<IEnumerable<Headquarter>> GetComboAsync(int cityId);

    Task<ActionResponse<Headquarter>> AddAsync(HeadquarterDTO headquarterDTO);

    Task<ActionResponse<Headquarter>> UpdateAsync(HeadquarterDTO headquarterDTO);

    Task<ActionResponse<Headquarter>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync();

    Task<ActionResponse<IEnumerable<Headquarter>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}