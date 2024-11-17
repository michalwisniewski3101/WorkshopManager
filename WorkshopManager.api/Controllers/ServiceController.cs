using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.api.Database;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly WorkshopContext _context;

    public ServiceController(WorkshopContext context)
    {
        _context = context;
    }

    // GET: api/Service
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return await _context.Services.ToListAsync();
    }

    // GET: api/Service/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(Guid id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null)
        {
            return NotFound();
        }

        return service;
    }



    [HttpPost("AddService")]
    public async Task<ActionResult<Service>> AddService([FromBody] Service service)
    {
        if (service == null)
        {
            return BadRequest("Nazwa usługi serwisowej nie może być pusta.");
        }

        service.Id = Guid.NewGuid();

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return Ok(service);
    }


    // PUT: api/Service/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutService(Guid id, Service service)
    {
        if (id != service.Id)
        {
            return BadRequest();
        }

        _context.Entry(service).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceExists(id))
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

    // DELETE: api/Service/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(Guid id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ServiceExists(Guid id)
    {
        return _context.Services.Any(e => e.Id == id);
    }
}
