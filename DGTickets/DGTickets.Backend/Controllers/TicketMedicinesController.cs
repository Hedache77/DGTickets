using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketMedicinesController : GenericController<TicketMedicine>
{
    private readonly ITicketMedicinesUnitOfWork _ticketMedicinesUnitOfWork;

    public TicketMedicinesController(IGenericUnitOfWork<TicketMedicine> unitOfWork, ITicketMedicinesUnitOfWork ticketMedicinesUnitOfWork) : base(unitOfWork)
    {
        _ticketMedicinesUnitOfWork = ticketMedicinesUnitOfWork;
    }

    [HttpGet("combo/{ticketId}")]
    public async Task<IActionResult> GetComboAsync(int ticketId)
    {
        return Ok(await _ticketMedicinesUnitOfWork.GetComboAsync(ticketId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(TicketMedicineDTO ticketMedicineDTO)
    {
        var action = await _ticketMedicinesUnitOfWork.AddAsync(ticketMedicineDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _ticketMedicinesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _ticketMedicinesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }
}