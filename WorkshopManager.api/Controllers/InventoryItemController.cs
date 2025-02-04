using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class InventoryItemController : ControllerBase
{
    private readonly WorkshopContext _context;

    public InventoryItemController(WorkshopContext context)
    {
        _context = context;
    }

    // GET: api/InventoryItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        return await _context.InventoryItems.ToListAsync();
    }

    // GET: api/InventoryItem/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryItem(Guid id)
    {
        var inventoryItem = await _context.InventoryItems.FindAsync(id);

        if (inventoryItem == null)
        {
            return NotFound();
        }

        return inventoryItem;
    }

    // POST: api/InventoryItem
    [HttpPost]
    public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInventoryItem), new { id = inventoryItem.Id }, inventoryItem);
    }
    [HttpPost("UpdateQuantityInStock/{id}")]
    public async Task<ActionResult<InventoryItem>> UpdateQuantityInStock(Guid id, int quantity)
    {
        var inventoryItem = await _context.InventoryItems.FindAsync(id);

        if (inventoryItem == null)
        {
            return NotFound();
        }

        
        inventoryItem.QuantityInStock += quantity;
        await _context.SaveChangesAsync();
        return inventoryItem;

    }
        // PUT: api/InventoryItem/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInventoryItem(Guid id, InventoryItem inventoryItem)
    {
        if (id != inventoryItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(inventoryItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InventoryItemExists(id))
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

    // DELETE: api/InventoryItem/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryItem(Guid id)
    {
        var inventoryItem = await _context.InventoryItems.FindAsync(id);
        if (inventoryItem == null)
        {
            return NotFound();
        }

        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool InventoryItemExists(Guid id)
    {
        return _context.InventoryItems.Any(e => e.Id == id);
    }
}
