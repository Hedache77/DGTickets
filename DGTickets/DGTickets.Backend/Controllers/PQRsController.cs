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
public class PQRsController : GenericController<PQR>
{
    private readonly IPQRsUnitOfWork _pqrsUnitOfWork;

    public PQRsController(IGenericUnitOfWork<PQR> unitOfWork, IPQRsUnitOfWork pqrsUnitOfWork) : base(unitOfWork)
    {
        _pqrsUnitOfWork = pqrsUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _pqrsUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _pqrsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _pqrsUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _pqrsUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("combo/{userId}")]
    public async Task<IActionResult> GetComboAsync(String userId)
    {
        return Ok(await _pqrsUnitOfWork.GetComboAsync(userId));
    }

    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _pqrsUnitOfWork.GetComboAsync());
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(PQRDTO pqrDTO)
    {
        pqrDTO.UserId = User.Identity!.Name!;
        var action = await _pqrsUnitOfWork.AddAsync(pqrDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(PQRDTO pqrDTO)
    {
        var action = await _pqrsUnitOfWork.UpdateAsync(pqrDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}