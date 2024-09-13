using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MechanicController : ControllerBase
{
    private readonly WorkshopContext _context;

    public MechanicController(WorkshopContext context)
    {
        _context = context;
    }

    // GET: api/Mechanic
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mechanic>>> GetMechanics()
    {
        return await _context.Mechanics.ToListAsync();
    }

    // GET: api/Mechanic/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Mechanic>> GetMechanic(Guid id)
    {
        var mechanic = await _context.Mechanics.FindAsync(id);

        if (mechanic == null)
        {
            return NotFound();
        }

        return mechanic;
    }

    // POST: api/Mechanic
    [HttpPost]
    public async Task<ActionResult<Mechanic>> PostMechanic(Mechanic mechanic)
    {
        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMechanic), new { id = mechanic.Id }, mechanic);
    }

    // PUT: api/Mechanic/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMechanic(Guid id, Mechanic mechanic)
    {
        if (id != mechanic.Id)
        {
            return BadRequest();
        }

        _context.Entry(mechanic).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MechanicExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Mechanic/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMechanic(Guid id)
    {
        var mechanic = await _context.Mechanics.FindAsync(id);
        if (mechanic == null)
        {
            return NotFound();
        }

        _context.Mechanics.Remove(mechanic);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MechanicExists(Guid id)
    {
        return _context.Mechanics.Any(e => e.Id == id);
    }
}
