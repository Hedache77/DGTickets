﻿using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;

namespace DGTickets.Backend.UnitsOfWork.Interfaces;

public interface IStatesUnitOfWork
{
    Task<IEnumerable<State>> GetComboAsync(int countryId);

    Task<IEnumerable<State>> GetComboAsync();

    Task<ActionResponse<State>> AddAsync(StateDTO stateDTO);

    Task<ActionResponse<State>> UpdateAsync(StateDTO stateDTO);

    Task<ActionResponse<State>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<State>>> GetAsync();

    Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}