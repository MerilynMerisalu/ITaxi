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

namespace WebApp.ApiControllers.AdminArea;
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
    {
        return Ok(await _appBLL.Vehicles.GettingOrderedVehiclesWithoutIncludesAsync());
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);

        if (vehicle == null) return NotFound();

        return _mapper.Map<Vehicle>(vehicle);
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicle(Guid id, Vehicle vehicle)
    {
        if (id != vehicle.Id) return BadRequest();
        var vehicleDTO = await _appBLL.Vehicles.FirstOrDefaultAsync(id);
        
        try
        {
            if (vehicleDTO != null)
            {
                vehicleDTO.VehicleAvailability = vehicle.VehicleAvailability;
                vehicleDTO.Id = vehicleDTO.Id;
                vehicleDTO.DriverId = vehicle.DriverId;
                vehicleDTO.ManufactureYear = vehicle.ManufactureYear;
                vehicleDTO.NumberOfSeats = vehicle.NumberOfSeats;
                vehicleDTO.VehicleMarkId = vehicle.VehicleMarkId;
                vehicleDTO.VehicleModelId = vehicle.VehicleModelId;
                vehicleDTO.VehiclePlateNumber = vehicle.VehiclePlateNumber;
                vehicleDTO.VehicleTypeId = vehicle.VehicleTypeId;
                vehicleDTO.UpdatedBy = User.GettingUserEmail();
                vehicleDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Vehicles.Update(vehicleDTO);
            }

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
    public async Task<ActionResult<Vehicle>> PostVehicle([FromBody]Vehicle vehicle)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
        vehicleDto.Id = Guid.NewGuid();
        vehicleDto.CreatedBy = User.GettingUserEmail();
        vehicleDto.UpdatedBy = User.GettingUserEmail();
        vehicleDto.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicleDto.UpdatedAt = DateTime.Now.ToUniversalTime();
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