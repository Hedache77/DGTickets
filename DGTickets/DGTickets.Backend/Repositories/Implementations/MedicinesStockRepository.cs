using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Repositories.Implementations;

public class MedicinesStockRepository : GenericRepository<MedicineStock>, IMedicinesStockRepository
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;

    public MedicinesStockRepository(DataContext context, IFileStorage fileStorage) : base(context)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public override async Task<ActionResponse<MedicineStock>> AddAsync(MedicineStock medicineStock)
    {
        var medicine = new MedicineStock
        {
            Name = medicineStock.Name,
            Quantity = medicineStock.Quantity,
            Manufacturer = medicineStock.Manufacturer,
            UnitOfMeasure = medicineStock.UnitOfMeasure,
            QuantityPerUnit = medicineStock.QuantityPerUnit,
            IsImageSquare = medicineStock.IsImageSquare
        };

        if (!string.IsNullOrEmpty(medicineStock.Image))
        {
            var imageBase64 = Convert.FromBase64String(medicineStock.Image!);
            medicine.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "medicines");
        }

        _context.Add(medicine);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = true,
                Result = medicine
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public override async Task<ActionResponse<MedicineStock>> UpdateAsync(MedicineStock medicineStock)
    {
        var currentMedicine = await _context.MedicinesStock.FindAsync(medicineStock.Id);
        if (currentMedicine == null)
        {
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        if (!string.IsNullOrEmpty(medicineStock.Image))
        {
            var imageBase64 = Convert.FromBase64String(medicineStock.Image!);
            currentMedicine.Image = await _fileStorage.SaveFileAsync(imageBase64, ".jpg", "medicines");
        }

        currentMedicine.Name = medicineStock.Name;
        currentMedicine.Quantity = medicineStock.Quantity;
        currentMedicine.Manufacturer = medicineStock.Manufacturer;
        currentMedicine.UnitOfMeasure = medicineStock.UnitOfMeasure;
        currentMedicine.QuantityPerUnit = medicineStock.QuantityPerUnit;
        currentMedicine.IsImageSquare = medicineStock.IsImageSquare;

        _context.Update(currentMedicine);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = true,
                Result = currentMedicine
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<MedicineStock>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public override async Task<ActionResponse<IEnumerable<MedicineStock>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.MedicinesStock
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<MedicineStock>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.MedicinesStock.AsQueryable();

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

    public async Task<IEnumerable<MedicineStock>> GetComboAsync()
    {
        return await _context.MedicinesStock
        .OrderBy(x => x.Name)
        .ToListAsync();
    }
}