﻿using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class TicketsController : GenericController<Ticket>
{
    private readonly ITicketsUnitOfWork _ticketsUnitOfWork;

    public TicketsController(IGenericUnitOfWork<Ticket> unitOfWork, ITicketsUnitOfWork ticketsUnitOfWork) : base(unitOfWork)
    {
        _ticketsUnitOfWork = ticketsUnitOfWork;
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _ticketsUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _ticketsUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _ticketsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(TicketDTO ticketDTO)
    {
        var action = await _ticketsUnitOfWork.AddAsync(ticketDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(TicketDTO ticketDTO)
    {
        var action = await _ticketsUnitOfWork.UpdateAsync(ticketDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}