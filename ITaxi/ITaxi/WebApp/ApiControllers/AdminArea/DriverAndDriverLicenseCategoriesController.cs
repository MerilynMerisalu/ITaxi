#nullable enable
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DriverAndDriverLicenseCategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DriverAndDriverLicenseCategoriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/DriverAndDriverLicenseCategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverAndDriverLicenseCategory>>> GetDriverAndDriverLicenseCategories()
    {
        return await _context.DriverAndDriverLicenseCategories.ToListAsync();
    }

    // GET: api/DriverAndDriverLicenseCategories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriverAndDriverLicenseCategory>> GetDriverAndDriverLicenseCategory(Guid id)
    {
        var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories.FindAsync(id);

        if (driverAndDriverLicenseCategory == null) return NotFound();

        return driverAndDriverLicenseCategory;
    }

    // PUT: api/DriverAndDriverLicenseCategories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriverAndDriverLicenseCategory(Guid id,
        DriverAndDriverLicenseCategory driverAndDriverLicenseCategory)
    {
        if (id != driverAndDriverLicenseCategory.Id) return BadRequest();

        _context.Entry(driverAndDriverLicenseCategory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriverAndDriverLicenseCategoryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/DriverAndDriverLicenseCategories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DriverAndDriverLicenseCategory>> PostDriverAndDriverLicenseCategory(
        DriverAndDriverLicenseCategory driverAndDriverLicenseCategory)
    {
        _context.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDriverAndDriverLicenseCategory", new {id = driverAndDriverLicenseCategory.Id},
            driverAndDriverLicenseCategory);
    }

    // DELETE: api/DriverAndDriverLicenseCategories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriverAndDriverLicenseCategory(Guid id)
    {
        var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories.FindAsync(id);
        if (driverAndDriverLicenseCategory == null) return NotFound();

        _context.DriverAndDriverLicenseCategories.Remove(driverAndDriverLicenseCategory);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverAndDriverLicenseCategoryExists(Guid id)
    {
        return _context.DriverAndDriverLicenseCategories.Any(e => e.Id == id);
    }
}