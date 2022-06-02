#nullable disable
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
    public class VehicleTypesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public VehicleTypesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/VehicleTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
        {
            return Ok(await _uow.VehicleTypes.GetAllAsync());
        }

        // GET: api/VehicleTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetVehicleType(Guid id)
        {
            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id);

            if (vehicleType == null)
            {
                return NotFound();
            }

            return vehicleType;
        }

        // PUT: api/VehicleTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleType(Guid id, VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return BadRequest();
            }

            

            try
            {
                _uow.VehicleTypes.Update(vehicleType);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(id))
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

        // POST: api/VehicleTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VehicleType>> PostVehicleType(VehicleType vehicleType)
        {
            _uow.VehicleTypes.Add(vehicleType);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetVehicleType", new { id = vehicleType.Id }, vehicleType);
        }

        // DELETE: api/VehicleTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleType(Guid id)
        {
            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            _uow.VehicleTypes.Remove(vehicleType);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleTypeExists(Guid id)
        {
            return _uow.VehicleTypes.Exists(id);
        }
    }
}
