using DGTickets.Backend.Data;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class RoleController: ControllerBase
{
    private readonly DataContext _context;

    public RoleController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Roles.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Role role)
    {
        _context.Add(role);
        await _context.SaveChangesAsync();
        return Ok(role);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Role role)
    {
        var currentRole = await _context.Roles.FindAsync(role.Id);
        if (currentRole == null)
        {
            return NotFound();
        }
        currentRole.Name = role.Name;
        _context.Update(currentRole);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        _context.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
