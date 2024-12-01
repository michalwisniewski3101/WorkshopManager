using Microsoft.AspNetCore.Mvc;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceController(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return Ok(await _serviceRepository.GetServicesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(Guid id)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(id);

        if (service == null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpGet("GetServiceSchedule/{id}")]
    public async Task<ActionResult<ServiceSchedule>> GetServiceSchedule(Guid id)
    {
        var serviceSchedule = await _serviceRepository.GetServiceScheduleByIdAsync(id);

        if (serviceSchedule == null)
        {
            return NotFound();
        }

        return Ok(serviceSchedule);
    }

    [HttpGet("GetServiceSchedulesByOrder/{id}")]
    public async Task<ActionResult<IEnumerable<ServiceSchedule>>> GetServiceSchedulesByOrder(Guid id)
    {
        var serviceSchedules = await _serviceRepository.GetServiceSchedulesByOrderIdAsync(id);
        return Ok(serviceSchedules);
    }

    [HttpPost("AddService")]
    public async Task<ActionResult<Guid>> AddService([FromBody] Service service)
    {
        if (service == null)
        {
            return BadRequest("Nazwa usługi serwisowej nie może być pusta.");
        }

        var id = await _serviceRepository.AddServiceAsync(service);
        return Ok(id);
    }

    [HttpPost("AddServiceSchedule/{id}")]
    public async Task<ActionResult<Guid>> AddServiceSchedule(Guid id, [FromBody] ServiceSchedule serviceSchedule)
    {
        if (serviceSchedule == null)
        {
            return BadRequest("Harmonogram serwisowy nie może być pusty.");
        }

        serviceSchedule.OrderId = id;
        var scheduleId = await _serviceRepository.AddServiceScheduleAsync(serviceSchedule);
        return Ok(scheduleId);
    }

    [HttpPut("UpdateServiceScheduleStatus/{id}")]
    public async Task<IActionResult> UpdateServiceScheduleStatus(Guid id, [FromBody] ServiceStatus newStatus)
    {
        try
        {
            await _serviceRepository.UpdateServiceScheduleStatusAsync(id, newStatus);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Wystąpił błąd podczas aktualizacji: {ex.Message}");
        }
    }
}
