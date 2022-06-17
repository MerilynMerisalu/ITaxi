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
    public class DrivesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public DrivesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Drives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drive>>> GetDrives()
        {
            return Ok( await _uow.Drives.GetAllDrivesWithoutIncludesAsync());
        }

        // GET: api/Drives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drive>> GetDrive(Guid id)
        {
            var drive = await _uow.Drives.GettingDriveWithoutIncludesAsync(id);

            if (drive == null)
            {
                return NotFound();
            }

            return drive;
        }

        // PUT: api/Drives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrive(Guid id, Drive drive)
        {
            if (id != drive.Id)
            {
                return BadRequest();
            }

            

            try
            {
                _uow.Drives.Update(drive);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriveExists(id))
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

        // POST: api/Drives
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Drive>> PostDrive(Drive drive)
        {
            _uow.Drives.Add(drive);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetDrive", new { id = drive.Id }, drive);
        }

        // DELETE: api/Drives/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrive(Guid id)
        {
            var drive = await _uow.Drives.FirstOrDefaultAsync(id);
            if (drive == null)
            {
                return NotFound();
            }

            _uow.Drives.Remove(drive);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool DriveExists(Guid id)
        {
            return _uow.Drives.Exists(id);
        }
    }
}
