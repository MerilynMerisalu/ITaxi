/*#nullable enable

using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;
[Route("api/DriverArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        
        if (vehicle == null) return NotFound();

        vehicle.CreatedAt = vehicle.CreatedAt.ToLocalTime();
        vehicle.UpdatedAt = vehicle.UpdatedAt.ToLocalTime();
        return vehicle;
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicle(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);

        if (vehicle!.Driver!.AppUserId != userId || !User.IsInRole(nameof(Admin)))
        {
            return Forbid();
        }

        try
        {
            vehicle.UpdatedBy = User.Identity!.Name;
            vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Vehicles.Update(vehicle);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }

        return NoContent();
    }



    // POST: api/Vehicles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
    {
        vehicle.Driver!.AppUserId = User.GettingUserId();
        vehicle.CreatedBy = User.Identity!.Name;
        vehicle.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicle.UpdatedBy = User.Identity!.Name;
        vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Vehicles.Add(vehicle);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicle", new {id = vehicle.Id}, vehicle);
    }

    // DELETE: api/Vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        if (await _appBLL.Schedules.AnyAsync(s => s != null && s.VehicleId.Equals(vehicle.Id))
            || await _appBLL.Bookings.AnyAsync(v => v != null && v.VehicleId.Equals(vehicle.Id)))
            return BadRequest("Vehicle cannot be deleted! ");
        
        _appBLL.Vehicles.Remove(vehicle);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /*private bool VehicleExists(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (userId != )
        {
            
        }
        return _appBLL.Vehicles.Exists(id);
    }#2#
}#1#*/