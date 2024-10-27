using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.api.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkshopManager.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MechanicSpecialtyController : ControllerBase
    {
        private readonly WorkshopContext _context;

        public MechanicSpecialtyController(WorkshopContext context)
        {
            _context = context;
        }

        // GET: api/MechanicSpecialty - Zwraca listę specjalności
        [HttpGet("GetSpecialties")]
        public async Task<ActionResult<IEnumerable<MechanicSpecialty>>> GetSpecialties()
        {
            return await _context.MechanicSpecialties.ToListAsync();
        }

        // POST: api/MechanicSpecialty/AddSpecialty - Dodaje nową specjalność
        [HttpPost("AddSpecialty")]
        public async Task<ActionResult<MechanicSpecialty>> AddSpecialty([FromBody] MechanicSpecialty specialty)
        {
            if (specialty == null || string.IsNullOrWhiteSpace(specialty.SpecialtyName))
            {
                return BadRequest("Nazwa specjalności nie może być pusta.");
            }

            specialty.Id = Guid.NewGuid();

            _context.MechanicSpecialties.Add(specialty);
            await _context.SaveChangesAsync();

            return Ok(specialty);
        }

    }
}
  

