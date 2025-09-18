using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.ApiControllers.AdminArea
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExtraServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExtraServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExtraService>>> GetExtraServices()
        {
            return await _context.ExtraServices.ToListAsync();
        }

        // GET: api/ExtraServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExtraService>> GetExtraService(Guid id)
        {
            var extraService = await _context.ExtraServices.FindAsync(id);

            if (extraService == null)
            {
                return NotFound();
            }

            return extraService;
        }

        // PUT: api/ExtraServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExtraService(Guid id, ExtraService extraService)
        {
            if (id != extraService.Id)
            {
                return BadRequest();
            }

            _context.Entry(extraService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExtraServiceExists(id))
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

        // POST: api/ExtraServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExtraService>> PostExtraService(ExtraService extraService)
        {
            _context.ExtraServices.Add(extraService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExtraService", new { id = extraService.Id }, extraService);
        }

        // DELETE: api/ExtraServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExtraService(Guid id)
        {
            var extraService = await _context.ExtraServices.FindAsync(id);
            if (extraService == null)
            {
                return NotFound();
            }

            _context.ExtraServices.Remove(extraService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExtraServiceExists(Guid id)
        {
            return _context.ExtraServices.Any(e => e.Id == id);
        }
    }
}
