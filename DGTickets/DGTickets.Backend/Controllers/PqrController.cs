using DGTickets.Backend.Data;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PqrController: ControllerBase
{
    private readonly DataContext _context;

    public PqrController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Pqrs.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var pqr = await _context.Pqrs.FindAsync(id);
        if (pqr == null)
        {
            return NotFound();
        }
        return Ok(pqr);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Pqr pqr)
    {
        _context.Add(pqr);
        await _context.SaveChangesAsync();
        return Ok(pqr);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Pqr pqr)
    {
        var currentPqr = await _context.Pqrs.FindAsync(pqr.Id);
        if (currentPqr == null)
        {
            return NotFound();
        }
        currentPqr.Description = pqr.Description;
        _context.Update(currentPqr);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var pqr = await _context.Pqrs.FindAsync(id);
        if (pqr == null)
        {
            return NotFound();
        }
        _context.Remove(pqr);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
