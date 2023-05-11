#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
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
    private readonly IMapper _mapper;
     
    public VehiclesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Vehicles
    [HttpGet]
    
    #warning change it later back
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesWithoutIncludesAsync(userId, roleName);
        
        return Ok(res.Select(v => _mapper.Map<App.Public.DTO.v1.AdminArea.Vehicle>(v)));
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);
        
        if (vehicle == null) return NotFound();

        vehicle.CreatedAt = vehicle.CreatedAt;
        vehicle.UpdatedAt = vehicle.UpdatedAt;
        return _mapper.Map<Vehicle>(vehicle);
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicle(Guid id, Vehicle vehicle)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicleDto = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);

        if (vehicleDto!.Driver!.AppUserId != userId || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        try
        {
            vehicleDto.VehicleAvailability = vehicle.VehicleAvailability;
            vehicleDto.ManufactureYear = vehicle.ManufactureYear;
            vehicleDto.NumberOfSeats = vehicle.NumberOfSeats;
            vehicleDto.VehicleMarkId = vehicle.VehicleMarkId;
            vehicleDto.VehicleModelId = vehicle.VehicleModelId;
            vehicleDto.VehiclePlateNumber = vehicle.VehiclePlateNumber;
            vehicleDto.VehicleTypeId = vehicle.VehicleTypeId;
            vehicleDto.UpdatedBy = User.Identity!.Name;
            vehicleDto.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Vehicles.Update(vehicleDto);
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
    public async Task<ActionResult<Vehicle>> PostVehicle([FromBody]Vehicle vehicle)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
        vehicleDto.Id = Guid.NewGuid();
        vehicleDto.DriverId = User.GettingUserId();
        vehicleDto.CreatedBy = User.Identity!.Name;
        vehicle.CreatedAt = DateTime.Now;
        vehicle.UpdatedBy = User.Identity!.Name;
        vehicle.UpdatedAt = DateTime.Now;
        
        _appBLL.Vehicles.Add(vehicleDto);
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
        return _appBLL.Vehicles.Exists(id);
    }
}