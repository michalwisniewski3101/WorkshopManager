using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkshopManager.api.Repos.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleController(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
    {
        var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
        return Ok(vehicles);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }


    [HttpPost]
    public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
    {
        var createdVehicle = await _vehicleRepository.AddVehicleAsync(vehicle);
        return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicle.Id }, createdVehicle);
    }
}
