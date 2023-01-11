#nullable disable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/AdminArea/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public VehiclesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Vehicles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetVehicles()
    {
        return Ok(await _appBLL.Vehicles.GettingOrderedVehiclesWithoutIncludesAsync());
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDTO>> GetVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);

        if (vehicle == null) return NotFound();

        return vehicle;
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicle(Guid id, VehicleDTO vehicle)
    {
        if (id != vehicle.Id) return BadRequest();

        try
        {
            _appBLL.Vehicles.Update(vehicle);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Vehicles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<VehicleDTO>> PostVehicle(VehicleDTO vehicle)
    {
        _appBLL.Vehicles.Add(vehicle);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicle", new {id = vehicle.Id}, vehicle);
    }

    // DELETE: api/Vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        _appBLL.Vehicles.Remove(vehicle);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }
}