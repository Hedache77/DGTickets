using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class HeadquarterMedicinesController : GenericController<HeadquarterMedicine>
{
    private readonly IHeadquarterMedicinesUnitOfWork _headquarterMedicinesUnitOfWork;

    public HeadquarterMedicinesController(IGenericUnitOfWork<HeadquarterMedicine> unitOfWork, IHeadquarterMedicinesUnitOfWork headquarterMedicinesUnitOfWork) : base(unitOfWork)
    {
        _headquarterMedicinesUnitOfWork = headquarterMedicinesUnitOfWork;
    }

    [HttpGet("combo/{headquarterId}")]
    public async Task<IActionResult> GetComboAsync(int headquarterId)
    {
        return Ok(await _headquarterMedicinesUnitOfWork.GetComboAsync(headquarterId));
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(HeadquarterMedicineDTO headquarterMedicineDTO)
    {
        var action = await _headquarterMedicinesUnitOfWork.AddAsync(headquarterMedicineDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _headquarterMedicinesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _headquarterMedicinesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }
}