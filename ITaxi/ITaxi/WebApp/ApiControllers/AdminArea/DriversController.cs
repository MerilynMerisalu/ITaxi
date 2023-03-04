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
public class DriversController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public DriversController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Drivers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverDTO>>> GetDrivers()
    {
        return Ok(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync());
    }

    // GET: api/Drivers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriverDTO>> GetDriver(Guid id)
    {
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id);

        if (driver == null) return NotFound();

        return driver;
    }

    // PUT: api/Drivers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriver(Guid id, DriverDTO driver)
    {
        if (id != driver.Id) return BadRequest();

        try
        {
            _appBLL.Drivers.Update(driver);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriverExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Drivers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DriverDTO>> PostDriver([FromBody]DriverDTO driver)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Drivers.Add(driver);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDriver", new
        {
            id = driver.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, driver);
    }

    // DELETE: api/Drivers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id);
        if (driver == null) return NotFound();

        _appBLL.Drivers.Remove(driver);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverExists(Guid id)
    {
        return _appBLL.Drivers.Exists(id);
    }
}