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
    public class DriverLicenseCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DriverLicenseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DriverLicenseCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverLicenseCategory>>> GetDriverLicenseCategories()
        {
            return await _context.DriverLicenseCategories.ToListAsync();
        }

        // GET: api/DriverLicenseCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverLicenseCategory>> GetDriverLicenseCategory(Guid id)
        {
            var driverLicenseCategory = await _context.DriverLicenseCategories.FindAsync(id);

            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            return driverLicenseCategory;
        }

        // PUT: api/DriverLicenseCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverLicenseCategory(Guid id, DriverLicenseCategory driverLicenseCategory)
        {
            if (id != driverLicenseCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(driverLicenseCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverLicenseCategoryExists(id))
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

        // POST: api/DriverLicenseCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverLicenseCategory>> PostDriverLicenseCategory(DriverLicenseCategory driverLicenseCategory)
        {
            _context.DriverLicenseCategories.Add(driverLicenseCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverLicenseCategory", new { id = driverLicenseCategory.Id }, driverLicenseCategory);
        }

        // DELETE: api/DriverLicenseCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverLicenseCategory(Guid id)
        {
            var driverLicenseCategory = await _context.DriverLicenseCategories.FindAsync(id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            _context.DriverLicenseCategories.Remove(driverLicenseCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverLicenseCategoryExists(Guid id)
        {
            return _context.DriverLicenseCategories.Any(e => e.Id == id);
        }
    }
}
