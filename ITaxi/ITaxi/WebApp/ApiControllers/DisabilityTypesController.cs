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
    public class DisabilityTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DisabilityTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DisabilityTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisabilityType>>> GetDisabilityTypes()
        {
            return await _context.DisabilityTypes.ToListAsync();
        }

        // GET: api/DisabilityTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisabilityType>> GetDisabilityType(Guid id)
        {
            var disabilityType = await _context.DisabilityTypes.FindAsync(id);

            if (disabilityType == null)
            {
                return NotFound();
            }

            return disabilityType;
        }

        // PUT: api/DisabilityTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisabilityType(Guid id, DisabilityType disabilityType)
        {
            if (id != disabilityType.Id)
            {
                return BadRequest();
            }

            _context.Entry(disabilityType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisabilityTypeExists(id))
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

        // POST: api/DisabilityTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DisabilityType>> PostDisabilityType(DisabilityType disabilityType)
        {
            _context.DisabilityTypes.Add(disabilityType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisabilityType", new { id = disabilityType.Id }, disabilityType);
        }

        // DELETE: api/DisabilityTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisabilityType(Guid id)
        {
            var disabilityType = await _context.DisabilityTypes.FindAsync(id);
            if (disabilityType == null)
            {
                return NotFound();
            }

            _context.DisabilityTypes.Remove(disabilityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DisabilityTypeExists(Guid id)
        {
            return _context.DisabilityTypes.Any(e => e.Id == id);
        }
    }
}
