using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class RatingsRepository : GenericRepository<Rating>, IRatingsRepository
{
    private readonly DataContext _context;

    public RatingsRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Rating>>> GetAsync()
    {
        var ratings = await _context.Ratings
            .OrderBy(c => c.Code)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Rating>>
        {
            WasSuccess = true,
            Result = ratings
        };
    }

    public override async Task<ActionResponse<Rating>> GetAsync(int id)
    {
        var rating = await _context.Ratings
             .FirstOrDefaultAsync(c => c.Id == id);

        if (rating == null)
        {
            return new ActionResponse<Rating>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Rating>
        {
            WasSuccess = true,
            Result = rating
        };
    }

    public override async Task<ActionResponse<IEnumerable<Rating>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Ratings
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Rating>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Code)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<IEnumerable<Rating>> GetComboAsync()
    {
        return await _context.Ratings
            .OrderBy(c => c.Code)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Ratings.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Code.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}