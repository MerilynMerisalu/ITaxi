#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DriverLicenseCategoriesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public DriverLicenseCategoriesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/DriverLicenseCategories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverLicenseCategoryDTO>>> GetDriverLicenseCategories()
    {
        return Ok(await _appBLL.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync());
    }

    // GET: api/DriverLicenseCategories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriverLicenseCategoryDTO>> GetDriverLicenseCategory(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);

        if (driverLicenseCategory == null) return NotFound();

        return driverLicenseCategory;
    }

    // PUT: api/DriverLicenseCategories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriverLicenseCategory(Guid id, DriverLicenseCategoryDTO driverLicenseCategory)
    {
        if (id != driverLicenseCategory.Id) return BadRequest();


        try
        {
            _appBLL.DriverLicenseCategories.Update(driverLicenseCategory);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriverLicenseCategoryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/DriverLicenseCategories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DriverLicenseCategoryDTO>> PostDriverLicenseCategory(
        [FromBody]DriverLicenseCategoryDTO driverLicenseCategory)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");

        }
        _appBLL.DriverLicenseCategories.Add(driverLicenseCategory);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDriverLicenseCategory", new
        {
            id = driverLicenseCategory.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, driverLicenseCategory);
    }

    // DELETE: api/DriverLicenseCategories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriverLicenseCategory(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);
        if (driverLicenseCategory == null) return NotFound();

        _appBLL.DriverLicenseCategories.Remove(driverLicenseCategory);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverLicenseCategoryExists(Guid id)
    {
        return _appBLL.DriverLicenseCategories.Exists(id);
    }
}