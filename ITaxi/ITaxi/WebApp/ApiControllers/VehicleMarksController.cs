#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMarksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleMarksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleMarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMark>>> GetVehicleMarks()
        {
            return await _context.VehicleMarks.ToListAsync();
        }

        // GET: api/VehicleMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMark>> GetVehicleMark(Guid id)
        {
            var vehicleMark = await _context.VehicleMarks.FindAsync(id);

            if (vehicleMark == null)
            {
                return NotFound();
            }

            return vehicleMark;
        }

        // PUT: api/VehicleMarks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleMark(Guid id, VehicleMark vehicleMark)
        {
            if (id != vehicleMark.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicleMark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleMarkExists(id))
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

        // POST: api/VehicleMarks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VehicleMark>> PostVehicleMark(VehicleMark vehicleMark)
        {
            _context.VehicleMarks.Add(vehicleMark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleMark", new { id = vehicleMark.Id }, vehicleMark);
        }

        // DELETE: api/VehicleMarks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMark(Guid id)
        {
            var vehicleMark = await _context.VehicleMarks.FindAsync(id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            _context.VehicleMarks.Remove(vehicleMark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleMarkExists(Guid id)
        {
            return _context.VehicleMarks.Any(e => e.Id == id);
        }
    }
}
