#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Domain;
using AutoMapper;
using Azure.Storage.Blobs;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehicle = App.Public.DTO.v1.DriverArea.Vehicle;

namespace WebApp.ApiControllers.DriverArea;

/// <summary>
/// Api controller for vehicles
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/DriverArea/[controller]")]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehiclesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
     
    /// <summary>
    /// Constructor for vehicles api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.DriverArea.VehicleDTO to Public.DTO.v1.DriverArea.Vehicle</param>
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName);
        
        return Ok(res.Select(v => _mapper.Map<Vehicle>(v)));
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        
        if (vehicle == null) return NotFound();

        vehicle.CreatedAt = vehicle.CreatedAt;
        vehicle.UpdatedAt = vehicle.UpdatedAt;
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicleDto = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);
        var currentDriver = await _appBLL.Drivers.GettingDriverByVehicleAsync(vehicleDto!.Id);
        
        if (currentDriver.AppUserId != userId && !User.IsInRole("Admin") )
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
            // System Managed Fields
            vehicleDto.UpdatedBy = User.Identity!.Name;
            vehicleDto.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Vehicles.Update(vehicleDto);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
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
        vehicleDto.DriverId = _appBLL.Drivers.GettingDriverByAppUserIdAsync(User.GettingUserId()).Result.Id;
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
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id);
        if (vehicle == null) return NotFound();

        if (await _appBLL.Vehicles.HasAnySchedulesAnyAsync(vehicle.Id)
            || await _appBLL.Vehicles.HasAnyBookingsAnyAsync(vehicle.Id))
            return BadRequest("Vehicle cannot be deleted! ");
        
        await _appBLL.Vehicles.RemoveAsync(vehicle.Id);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }
    
    /// <summary>
    /// Gets all manufacture years for driver area vehicles create and edit
    /// </summary>
    /// <returns>List of ints</returns>
    [Route("GetManufactureYears")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<int>> GetManufactureYears()
    {
        var res =  _appBLL.Vehicles.GettingManufactureYears();
        
        return Ok(res);
    }
    
    // <summary>
    /// Gets all manufacture years for driver area vehicles create and edit
    /// </summary>
    /// <returns>List of ints</returns>
    [Route("Gallery/{vehicleId:guid}")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Photo>> Gallery(
        [FromRoute]Guid vehicleId,IFormFile file)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(vehicleId);
        if (vehicle == null) return NotFound();

        
        var client = new BlobServiceClient("UseDevelopmentStorage=true");
        var container = client.GetBlobContainerClient("vehicles");
        var blob = container.GetBlobClient(file.FileName);
        await blob.UploadAsync(file.OpenReadStream());

        //create photo entity in database

        var photo = new Photo()
        {
            //Vehicle = vehicle,
            Title = file.FileName,
            PhotoURL = blob.Uri.ToString(),
        };

        //await _appBLL.SaveChangesAsync();
        
        return photo;
    }
}

