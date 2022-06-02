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
    public class DriverLicenseCategoriesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public DriverLicenseCategoriesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/DriverLicenseCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverLicenseCategory>>> GetDriverLicenseCategories()
        {
            return Ok(await _uow.DriverLicenseCategories.GetAllAsync());
        }

        // GET: api/DriverLicenseCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverLicenseCategory>> GetDriverLicenseCategory(Guid id)
        {
            var driverLicenseCategory = await _uow.DriverLicenseCategories.FirstOrDefaultAsync(id);

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

            

            try
            {
                _uow.DriverLicenseCategories.Update(driverLicenseCategory);
                await _uow.SaveChangesAsync();
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
            _uow.DriverLicenseCategories.Add(driverLicenseCategory);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetDriverLicenseCategory", new { id = driverLicenseCategory.Id }, driverLicenseCategory);
        }

        // DELETE: api/DriverLicenseCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverLicenseCategory(Guid id)
        {
            var driverLicenseCategory = await _uow.DriverLicenseCategories.FirstOrDefaultAsync(id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            _uow.DriverLicenseCategories.Remove(driverLicenseCategory);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverLicenseCategoryExists(Guid id)
        {
            return _uow.DriverLicenseCategories.Exists(id);
        }
    }
}
