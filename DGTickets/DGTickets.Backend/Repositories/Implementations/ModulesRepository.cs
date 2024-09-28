using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class ModulesRepository : GenericRepository<Module>, IModulesRepository
{
    private readonly DataContext _context;

    public ModulesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Module>>> GetAsync()
    {
        var modules = await _context.Modules
            .Include(x => x.Headquarter)
            .OrderBy(x => x.Name)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Module>>
        {
            WasSuccess = true,
            Result = modules
        };
    }

    public override async Task<ActionResponse<Module>> GetAsync(int id)
    {
        var module = await _context.Modules
             .Include(x => x.Headquarter)
             .FirstOrDefaultAsync(c => c.Id == id);

        if (module == null)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Module>
        {
            WasSuccess = true,
            Result = module
        };
    }

    public override async Task<ActionResponse<IEnumerable<Module>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Modules
            .Include(x => x.Headquarter)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Module>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<Module>> AddAsync(ModuleDTO moduleDTO)
    {
        var headquarter = await _context.Headquarters.FindAsync(moduleDTO.HeadquarterId);
        if (headquarter == null)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR006"
            };
        }

        var module = new Module
        {
            Headquarter = headquarter,
            Name = moduleDTO.Name,
        };

        _context.Add(module);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Module>
            {
                WasSuccess = true,
                Result = module
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<IEnumerable<Module>> GetComboAsync(int headquarterId)
    {
        return await _context.Modules
            .Where(x => x.HeadquarterId == headquarterId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Module>> GetComboAsync()
    {
        return await _context.Modules
        .OrderBy(x => x.Name)
        .ToListAsync();
    }

    public async Task<ActionResponse<Module>> UpdateAsync(ModuleDTO moduleDTO)
    {
        var currentModule = await _context.Modules.FindAsync(moduleDTO.Id);
        if (currentModule == null)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR007"
            };
        }

        var headquarter = await _context.Headquarters.FindAsync(moduleDTO.HeadquarterId);
        if (headquarter == null)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR006"
            };
        }

        currentModule.Headquarter = headquarter;
        currentModule.Name = moduleDTO.Name;

        _context.Update(currentModule);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Module>
            {
                WasSuccess = true,
                Result = currentModule
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Module>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Modules.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}