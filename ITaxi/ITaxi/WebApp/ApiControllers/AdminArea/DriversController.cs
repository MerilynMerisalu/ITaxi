/*#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DriversController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public DriversController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Drivers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
    {
        return Ok(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync());
    }

    // GET: api/Drivers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> GetDriver(Guid id)
    {
        var driver = await _uow.Drivers.FirstOrDefaultAsync(id);

        if (driver == null) return NotFound();

        return driver;
    }

    // PUT: api/Drivers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriver(Guid id, Driver driver)
    {
        if (id != driver.Id) return BadRequest();

        try
        {
            _uow.Drivers.Update(driver);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<Driver>> PostDriver(Driver driver)
    {
        _uow.Drivers.Add(driver);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetDriver", new {id = driver.Id}, driver);
    }

    // DELETE: api/Drivers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        var driver = await _uow.Drivers.FirstOrDefaultAsync(id);
        if (driver == null) return NotFound();

        _uow.Drivers.Remove(driver);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverExists(Guid id)
    {
        return _uow.Drivers.Exists(id);
    }
}*/