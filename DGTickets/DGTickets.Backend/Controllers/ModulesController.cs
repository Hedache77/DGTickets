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
public class ModulesController : GenericController<Module>
{
    private readonly IModulesUnitOfWork _modulesUnitOfWork;

    public ModulesController(IGenericUnitOfWork<Module> unitOfWork, IModulesUnitOfWork modulesUnitOfWork) : base(unitOfWork)
    {
        _modulesUnitOfWork = modulesUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _modulesUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _modulesUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
    {
        var response = await _modulesUnitOfWork.GetAsync(pagination);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _modulesUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("combo/{headquarterId:int}")]
    public async Task<IActionResult> GetComboAsync(int headquarterId)
    {
        return Ok(await _modulesUnitOfWork.GetComboAsync(headquarterId));
    }

    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _modulesUnitOfWork.GetComboAsync());
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(ModuleDTO moduleDTO)
    {
        var action = await _modulesUnitOfWork.AddAsync(moduleDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(ModuleDTO moduleDTO)
    {
        var action = await _modulesUnitOfWork.UpdateAsync(moduleDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}