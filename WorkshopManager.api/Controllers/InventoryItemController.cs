using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos;
using WorkshopManager.api.Repos.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class InventoryItemController : ControllerBase
{
    private readonly WorkshopContext _context;
    private readonly IServiceRepository _serviceRepository;

    public InventoryItemController(WorkshopContext context, IServiceRepository serviceRepository)
    {
        _context = context;
        _serviceRepository = serviceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        return await _context.InventoryItems.ToListAsync();
    }

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
    [HttpGet("GetLowStockInventoryItems")]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetLowStockInventoryItems()
    {
        return await _context.InventoryItems
            .Where(item => item.QuantityInStock <= item.ReorderLevel)
            .ToListAsync();
    }
    [HttpGet("GetMissingInventoryItems")]
    public async Task<ActionResult<IEnumerable<object>>> GetMissingInventoryItems()
    {
        var missingItems = await GetMissingItemsAsync();
        return Ok(missingItems);
    }


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

        
        

        var missingInventoryItems = await GetMissingItemsAsync();
        var missingInventoryItem = missingInventoryItems.FirstOrDefault(mi => mi.Id == id);

        inventoryItem.QuantityInStock += quantity;

        if (missingInventoryItem != null && missingInventoryItem.MissingStock >= quantity)
        {
            var serviceSchedules = await _context.ServiceSchedules
                .Where(s => s.OrderItems.Any(oi => oi.InventoryItemId == inventoryItem.Id))
                .ToListAsync();

            missingInventoryItems.Remove(missingInventoryItem);

            foreach (var service in serviceSchedules)
            {
                foreach (var item in service.OrderItems)
                {
                    if (item.InventoryItemId == inventoryItem.Id)
                    {
                        inventoryItem.QuantityInStock -= item.Quantity;
                    }
                }

     
                bool hasMissingItems = service.OrderItems.Any(oi =>
                    missingInventoryItems.Any(mi => mi.Id == oi.InventoryItemId));

                if (!hasMissingItems)
                {
                   await _serviceRepository.UpdateServiceScheduleStatusAsync(service.Id, ServiceStatus.Pending);
                }
            }
        }






        await _context.SaveChangesAsync();
        return inventoryItem;

    }

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



    private async Task<List<MissingInventoryItemDto>> GetMissingItemsAsync()
    {
        var groupedOrderItems = await _context.ServiceSchedules
            .SelectMany(s => s.OrderItems)
            .GroupBy(oi => oi.InventoryItemId)
            .Select(g => new
            {
                InventoryItemId = g.Key,
                TotalQuantity = g.Sum(oi => oi.Quantity)
            })
            .ToListAsync();

        var inventoryItems = await _context.InventoryItems.ToListAsync();

        var missingItems = inventoryItems
            .Select(inv => new MissingInventoryItemDto
            {
                Id = inv.Id,
                Name = inv.Name,
                Description = inv.Description,
                ProductNumber = inv.ProductNumber,
                QuantityInStock = inv.QuantityInStock,
                UnitPrice = inv.UnitPrice,
                ReorderLevel = inv.ReorderLevel,
                Supplier = inv.Supplier,
                TaxRate = inv.TaxRate,
                MissingStock = groupedOrderItems
                    .Where(gr => gr.InventoryItemId == inv.Id)
                    .Select(gr => gr.TotalQuantity - inv.QuantityInStock)
                    .FirstOrDefault()
            })
            .Where(x => x.MissingStock > 0)
            .ToList();

        return missingItems;
    }

    public class MissingInventoryItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductNumber { get; set; }
        public int QuantityInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public int ReorderLevel { get; set; }
        public string Supplier { get; set; }
        public int TaxRate { get; set; }
        public int MissingStock { get; set; }
    }
}
