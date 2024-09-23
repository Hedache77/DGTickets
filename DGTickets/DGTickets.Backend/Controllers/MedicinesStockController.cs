using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
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

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _medicinesStockUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _medicinesStockUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }
}