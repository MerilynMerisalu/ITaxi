#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
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
        private readonly IAppUnitOfWork _uow;

        public VehicleMarksController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/VehicleMarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMark>>> GetVehicleMarks()
        {
            return Ok(await _uow.VehicleMarks.GetAllVehicleMarkOrderedAsync());
        }

        // GET: api/VehicleMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMark>> GetVehicleMark(Guid id)
        {
            var vehicleMark = await _uow.VehicleMarks.FirstOrDefaultAsync(id);

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
            
            try
            {
                _uow.VehicleMarks.Update(vehicleMark);
                await _uow.SaveChangesAsync();
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
            _uow.VehicleMarks.Add(vehicleMark);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetVehicleMark", new { id = vehicleMark.Id }, vehicleMark);
        }

        // DELETE: api/VehicleMarks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMark(Guid id)
        {
            var vehicleMark = await _uow.VehicleMarks.FirstOrDefaultAsync(id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            _uow.VehicleMarks.Remove(vehicleMark);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleMarkExists(Guid id)
        {
            return _uow.VehicleMarks.Exists(id);
        }
    }
}
