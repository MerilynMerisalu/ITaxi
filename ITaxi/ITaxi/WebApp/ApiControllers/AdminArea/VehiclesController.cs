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

/// <summary>
/// Api controller for vehicles
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehiclesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for vehicles api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.VehicleDTO to Public.DTO.v1.AdminArea.Vehicle</param>
    public VehiclesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Vehicles
    /// <summary>
    /// Gets all the vehicles
    /// </summary>
    /// <returns>List of vehicles with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Vehicle>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
    {
        return Ok(await _appBLL.Vehicles.GettingOrderedVehiclesAsync());
    }

    // GET: api/Vehicles/5
    /// <summary>
    /// Returns vehicle based on id
    /// </summary>
    /// <param name="id">Vehicle id, Guid</param>
    /// <returns>Vehicle (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);

        if (vehicle == null) return NotFound();

        return _mapper.Map<Vehicle>(vehicle);
    }

    // PUT: api/Vehicles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a vehicle
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="vehicle">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// <summary>
    /// Creating a new vehicle
    /// </summary>
    /// <param name="vehicle">Vehicle with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        await _appBLL.Vehicles.RemoveAsync(vehicle.Id);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }
}