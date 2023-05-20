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
/// Api controller for vehicle marks
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehicleMarksController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructor for vehicle marks api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.VehicleMarksDTO to 
    /// Public.DTO.v1.AdminArea.VehicleMark</param>
    public VehicleMarksController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/VehicleMarks
    /// <summary>
    /// Gets all the vehicle marks
    /// </summary>
    /// <returns>List of vehicle marks with statusCode 200 or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<VehicleMark>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<VehicleMark>>> GetVehicleMarks()
    {
        var res = await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync();
        return Ok(res.Select(v => _mapper.Map<VehicleMark>(v)));
    }

    // GET: api/VehicleMarks/5
    /// <summary>
    /// Returns vehicle mark based on id
    /// </summary>
    /// <param name="id">Vehicle mark id, Guid</param>
    /// <returns>Vehicle mark (TEntity) with statusCode 200 or status404 or Status403 or Status401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleMark), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleMark>> GetVehicleMark(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);

        if (vehicleMark == null) return NotFound();

        return _mapper.Map<VehicleMark>(vehicleMark);
    }

    // PUT: api/VehicleMarks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a vehicle mark
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="vehicleMark">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutVehicleMark(Guid id, VehicleMark vehicleMark)
    {
        if (id != vehicleMark.Id) return BadRequest();

        var vehicleMarkDto = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);
        
        try
        {
            if (vehicleMarkDto != null)
            {
                vehicleMarkDto.VehicleMarkName = vehicleMark.VehicleMarkName;
                vehicleMarkDto.UpdatedBy = User.GettingUserEmail();
                vehicleMarkDto.UpdatedAt = DateTime.Now.ToUniversalTime();
            }

            if (vehicleMarkDto != null) _appBLL.VehicleMarks.Update(vehicleMarkDto);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleMarkExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/VehicleMarks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new vehicle mark
    /// </summary>
    /// <param name="vehicleMark">VehicleMarkDTO with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(VehicleMark), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleMark>> PostVehicleMark([FromBody]VehicleMark vehicleMark)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var vehicleMarkDto = new VehicleMarkDTO();
        vehicleMarkDto.VehicleMarkName = vehicleMark.VehicleMarkName;
        vehicleMarkDto.CreatedBy = User.GettingUserEmail();
        vehicleMarkDto.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicleMarkDto.UpdatedBy = User.GettingUserEmail();
        vehicleMarkDto.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.VehicleMarks.Add(vehicleMarkDto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleMark", new
        {
            id = vehicleMark.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, vehicleMark);
    }

    // DELETE: api/VehicleMarks/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204, StatusCode 404, StatusCode 403, StatusCode 401</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteVehicleMark(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);
        if (vehicleMark == null) return NotFound();

        _appBLL.VehicleMarks.Remove(vehicleMark);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>Boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool VehicleMarkExists(Guid id)
    {
        return _appBLL.VehicleMarks.Exists(id);
    }
}