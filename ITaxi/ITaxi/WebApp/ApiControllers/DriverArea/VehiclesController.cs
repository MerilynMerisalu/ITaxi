#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;
[ApiController]
[Route("api/v{version:apiVersion}/DriverArea/[controller]")]
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
    [AllowAnonymous] 
    #warning change it later back
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetVehicles()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDTO>> GetVehicle(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        
        if (vehicle == null) return NotFound();

        vehicle.CreatedAt = vehicle.CreatedAt;
        vehicle.UpdatedAt = vehicle.UpdatedAt;
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

        if (vehicle!.Driver!.AppUserId != userId || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        try
        {
            vehicle.UpdatedBy = User.Identity!.Name;
            vehicle.UpdatedAt = DateTime.Now;
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
    public async Task<ActionResult<VehicleDTO>> PostVehicle([FromBody]VehicleDTO vehicle)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        vehicle.Driver!.AppUserId = User.GettingUserId();
        vehicle.CreatedBy = User.Identity!.Name;
        vehicle.CreatedAt = DateTime.Now;
        vehicle.UpdatedBy = User.Identity!.Name;
        vehicle.UpdatedAt = DateTime.Now;
        _appBLL.Vehicles.Add(vehicle);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicle", new
        {
            id = vehicle.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, vehicle);
    }

    // DELETE: api/Vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        if (await _appBLL.Vehicles.HasAnySchedulesAnyAsync(vehicle.Id)
            || await _appBLL.Vehicles.HasAnyBookingsAnyAsync(vehicle.Id))
            return BadRequest("Vehicle cannot be deleted! ");
        
        _appBLL.Vehicles.Remove(vehicle);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleExists(Guid id)
    { 
        /*var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();*/
        return _appBLL.Vehicles.Exists(id);
    }
}