#nullable enable

using App.Contracts.DAL;
using App.Domain;
using App.Domain.DTO;
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
    private readonly IAppUnitOfWork _uow;

     
    public VehiclesController(IAppUnitOfWork context)
    {
        _uow = context;
    }

    // GET: api/Vehicles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Vehicles.GettingOrderedVehiclesWithoutIncludesAsync(userId, roleName);
        foreach (var vehicle in res)
        {
            vehicle.CreatedAt = vehicle.CreatedAt.ToLocalTime();
            vehicle.UpdatedAt = vehicle.UpdatedAt.ToLocalTime();
        }
        return Ok(res);
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _uow.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);
        
        if (vehicle == null) return NotFound();

        vehicle.CreatedAt = vehicle.CreatedAt.ToLocalTime();
        vehicle.UpdatedAt = vehicle.UpdatedAt.ToLocalTime();
        return vehicle;
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicle(Guid id, VehicleDTO vehicleDTO)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _uow.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);
        
        if (roleName.Equals("Admin"))
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.ManufactureYear = vehicleDTO.ManufactureYear;
                    vehicle.VehicleAvailability = vehicleDTO.VehicleAvailability;
                    vehicle.VehicleMarkId = vehicleDTO.VehicleMarkId;
                    vehicle.VehicleModelId = vehicleDTO.VehicleModelId;
                    vehicle.VehicleTypeId = vehicleDTO.VehicleTypeId;
                    vehicle.VehiclePlateNumber = vehicleDTO.VehiclePlateNumber;
                    vehicle.NumberOfSeats = vehicleDTO.NumberOfSeats;
                    vehicle.UpdatedBy = User.Identity!.Name;
                    vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Vehicles.Update(vehicle);
                }

                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }
        else
        {
            if (vehicle.Driver!.AppUserId != userId)
            {
                return Forbid();
            }
            else
            {
                try
                {
                    vehicle.ManufactureYear = vehicleDTO.ManufactureYear;
                    vehicle.VehicleAvailability = vehicleDTO.VehicleAvailability;
                    vehicle.VehicleMarkId = vehicleDTO.VehicleMarkId;
                    vehicle.VehicleModelId = vehicleDTO.VehicleModelId;
                    vehicle.VehicleTypeId = vehicleDTO.VehicleTypeId;
                    vehicle.VehiclePlateNumber = vehicleDTO.VehiclePlateNumber;
                    vehicle.NumberOfSeats = vehicleDTO.NumberOfSeats;
                    vehicle.UpdatedBy = User.Identity!.Name;
                    vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Vehicles.Update(vehicle);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(id))
                        return NotFound();
                    throw;
                }

                return NoContent();
            }
        }
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
        _uow.Vehicles.Add(vehicle);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetVehicle", new {id = vehicle.Id}, vehicle);
    }

    // DELETE: api/Vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _uow.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        if (await _uow.Schedules.AnyAsync(s => vehicle != null && s != null && s.VehicleId.Equals(vehicle.Id))
            || await _uow.Bookings.AnyAsync(v => vehicle != null && v != null && v.VehicleId.Equals(vehicle.Id)))
            return BadRequest("Vehicle cannot be deleted! ");
        
        _uow.Vehicles.Remove(vehicle);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleExists(Guid id)
    {
        return _uow.Vehicles.Exists(id);
    }
}