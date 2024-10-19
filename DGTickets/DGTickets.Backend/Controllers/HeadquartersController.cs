using DGTickets.Backend.UnitsOfWork.Implementations;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeadquartersController : GenericController<Headquarter>
{
    private readonly IHeadquartersUnitOfWork _headquartersUnitOfWork;

    public HeadquartersController(IGenericUnitOfWork<Headquarter> unitOfWork, IHeadquartersUnitOfWork headquartersUnitOfWork) : base(unitOfWork)
    {
        _headquartersUnitOfWork = headquartersUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _headquartersUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _headquartersUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _headquartersUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _headquartersUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("combo/{cityId:int}")]
    public async Task<IActionResult> GetComboAsync(int cityId)
    {
        return Ok(await _headquartersUnitOfWork.GetComboAsync(cityId));
    }

    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _headquartersUnitOfWork.GetComboAsync());
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(HeadquarterDTO headquarterDTO)
    {
        var action = await _headquartersUnitOfWork.AddAsync(headquarterDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(HeadquarterDTO headquarterDTO)
    {
        var action = await _headquartersUnitOfWork.UpdateAsync(headquarterDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}