﻿using DGTickets.Backend.UnitsOfWork.Implementations;
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
public class TicketsController : GenericController<Ticket>
{
    private readonly ITicketsUnitOfWork _ticketsUnitOfWork;

    public TicketsController(IGenericUnitOfWork<Ticket> unitOfWork, ITicketsUnitOfWork ticketsUnitOfWork) : base(unitOfWork)
    {
        _ticketsUnitOfWork = ticketsUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _ticketsUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
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

    [HttpGet("combo/{headquarterId:int}")]
    public async Task<IActionResult> GetComboAsync(int headquarterId)
    {
        return Ok(await _ticketsUnitOfWork.GetComboAsync(headquarterId));
    }

    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _ticketsUnitOfWork.GetComboAsync());
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(TicketDTO ticketDTO)
    {
        var action = await _ticketsUnitOfWork.AddAsync(ticketDTO);

        //string html = "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Document</title> </head> <body> <header style=\"width: 100%; height: 100px; background-color: #207870; color: white; font-size: 50px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center;\"> DGTICKETS </header> <section style=\"width: 100%; height: 200px; background-color: #f8f9f9; color: black; font-size: 20px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0 auto; text-align: center; margin-top: 50px;\"> <h1>THANK YOU FOR YOUR TICKET</h1> <h2>YOUR ORDER HAS BEEN CONFIRMED</h2> <h6>YOUR TICKET NUMBER IS: 123456</h6> </section> </body> </html>";

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