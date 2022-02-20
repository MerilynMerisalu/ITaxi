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
    public class RideTimesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RideTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RideTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RideTime>>> GetRideTimes()
        {
            return await _context.RideTimes.ToListAsync();
        }

        // GET: api/RideTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RideTime>> GetRideTime(Guid id)
        {
            var rideTime = await _context.RideTimes.FindAsync(id);

            if (rideTime == null)
            {
                return NotFound();
            }

            return rideTime;
        }

        // PUT: api/RideTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRideTime(Guid id, RideTime rideTime)
        {
            if (id != rideTime.Id)
            {
                return BadRequest();
            }

            _context.Entry(rideTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideTimeExists(id))
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

        // POST: api/RideTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RideTime>> PostRideTime(RideTime rideTime)
        {
            _context.RideTimes.Add(rideTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRideTime", new { id = rideTime.Id }, rideTime);
        }

        // DELETE: api/RideTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRideTime(Guid id)
        {
            var rideTime = await _context.RideTimes.FindAsync(id);
            if (rideTime == null)
            {
                return NotFound();
            }

            _context.RideTimes.Remove(rideTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RideTimeExists(Guid id)
        {
            return _context.RideTimes.Any(e => e.Id == id);
        }
    }
}
