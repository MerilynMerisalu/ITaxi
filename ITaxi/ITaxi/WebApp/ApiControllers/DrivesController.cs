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
    public class DrivesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DrivesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Drives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drive>>> GetDrives()
        {
            return await _context.Drives.ToListAsync();
        }

        // GET: api/Drives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drive>> GetDrive(Guid id)
        {
            var drive = await _context.Drives.FindAsync(id);

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

            _context.Entry(drive).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Drives.Add(drive);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrive", new { id = drive.Id }, drive);
        }

        // DELETE: api/Drives/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrive(Guid id)
        {
            var drive = await _context.Drives.FindAsync(id);
            if (drive == null)
            {
                return NotFound();
            }

            _context.Drives.Remove(drive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriveExists(Guid id)
        {
            return _context.Drives.Any(e => e.Id == id);
        }
    }
}
