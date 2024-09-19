using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesStockController : GenericController<MedicineStock>
{
    private readonly IMedicinesStockUnitOfWork _medicinesStockUnitOfWork;

    public MedicinesStockController(IGenericUnitOfWork<MedicineStock> unitOfWork, IMedicinesStockUnitOfWork medicinesStockUnitOfWork) : base(unitOfWork)
    {
        _medicinesStockUnitOfWork = medicinesStockUnitOfWork;
    }

    [HttpPost("full")]
    public override async Task<IActionResult> PostAsync(MedicineStock medicineStock)
    {
        var action = await _medicinesStockUnitOfWork.AddAsync(medicineStock);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public override async Task<IActionResult> PutAsync(MedicineStock medicineStock)
    {
        var action = await _medicinesStockUnitOfWork.UpdateAsync(medicineStock);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}