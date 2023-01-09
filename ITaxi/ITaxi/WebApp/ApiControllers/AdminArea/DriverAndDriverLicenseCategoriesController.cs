#nullable enable
using App.BLL;
using App.BLL.DTO.AdminArea;

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
    private readonly AppBLL _appBLL;

    public DriverAndDriverLicenseCategoriesController( AppBLL appBLL)
    {
        _appBLL = appBLL;
        
    }

    // GET: api/DriverAndDriverLicenseCategories
    [HttpGet]
    public async Task<IEnumerable<DriverAndDriverLicenseCategoryDTO>> GetDriverAndDriverLicenseCategories()
    {
        return await _appBLL.DriverAndDriverLicenseCategories.GetAllAsync();
    }

    // GET: api/DriverAndDriverLicenseCategories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriverAndDriverLicenseCategoryDTO>> GetDriverAndDriverLicenseCategory(Guid id)
    {
        var driverAndDriverLicenseCategory = await _appBLL.DriverAndDriverLicenseCategories.FirstOrDefaultAsync(id);

        if (driverAndDriverLicenseCategory == null) return NotFound();

        return driverAndDriverLicenseCategory;
    }

    // PUT: api/DriverAndDriverLicenseCategories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriverAndDriverLicenseCategory(Guid id,
        DriverAndDriverLicenseCategoryDTO driverAndDriverLicenseCategory)
    {
        if (id != driverAndDriverLicenseCategory.Id) return BadRequest();

        

        try
        {
            _appBLL.DriverAndDriverLicenseCategories.Update(driverAndDriverLicenseCategory);
            await _appBLL.SaveChangesAsync();
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
    public async Task<ActionResult<DriverAndDriverLicenseCategoryDTO>> PostDriverAndDriverLicenseCategory(
        DriverAndDriverLicenseCategoryDTO driverAndDriverLicenseCategory)
    {
        _appBLL.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDriverAndDriverLicenseCategory", new {id = driverAndDriverLicenseCategory.Id},
            driverAndDriverLicenseCategory);
    }

    // DELETE: api/DriverAndDriverLicenseCategories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriverAndDriverLicenseCategory(Guid id)
    {
        var driverAndDriverLicenseCategory = await _appBLL.DriverAndDriverLicenseCategories.FirstOrDefaultAsync(id);
        if (driverAndDriverLicenseCategory == null) return NotFound();

        _appBLL.DriverAndDriverLicenseCategories.Remove(driverAndDriverLicenseCategory);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverAndDriverLicenseCategoryExists(Guid id)
    {
        return _appBLL.DriverAndDriverLicenseCategories.Any(e => e.Id == id);
    }
}