using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class HeadquarterMedicinesRepository : GenericRepository<HeadquarterMedicine>, IHeadquarterMedicinesRepository
{
    private readonly DataContext _context;

    public HeadquarterMedicinesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HeadquarterMedicine>> GetComboAsync(int headquarterId)
    {
        return await _context.HeadquarterMedicines
            .Include(x => x.Medicine)
            .Where(x => x.HeadquarterId == headquarterId)
            .OrderBy(x => x.Medicine.Name)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.HeadquarterMedicines.AsQueryable();
        queryable = queryable.Where(x => x.HeadquarterId == pagination.Id);

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Medicine.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public override async Task<ActionResponse<IEnumerable<HeadquarterMedicine>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.HeadquarterMedicines
            .Include(x => x.Medicine)
            .AsQueryable();
        queryable = queryable.Where(x => x.HeadquarterId == pagination.Id);

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Medicine.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<HeadquarterMedicine>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Medicine.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
}