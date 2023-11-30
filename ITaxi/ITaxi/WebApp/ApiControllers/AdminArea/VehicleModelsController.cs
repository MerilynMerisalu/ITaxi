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
/// Api controller for vehicle models
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehicleModelsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for vehicle models api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.VehicleMarkDTO to Public.DTO.v1.AdminArea.VehicleMark</param>
    public VehicleModelsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/VehicleModels
    /// <summary>
    /// Gets all the vehicle models
    /// </summary>
    /// <returns>List of vehicle models with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<VehicleModel>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<VehicleModel>>> GetVehicleModels()
    {
        var res = await _appBLL.VehicleModels.GetAllAsync();
        return Ok(res.Select(v => _mapper.Map<VehicleModel>(v)));
    }

    // GET: api/VehicleModels/5
    /// <summary>
    /// Returns vehicle model based on id
    /// </summary>
    /// <param name="id">Vehicle model id, Guid</param>
    /// <returns>Vehicle model (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleModel), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleModel>> GetVehicleModel(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(id);

        if (vehicleModel == null) return NotFound();

        return _mapper.Map<VehicleModel>(vehicleModel);
    }

    // PUT: api/VehicleModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a vehicle model
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="vehicleModel">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutVehicleModel(Guid id, VehicleModel vehicleModel)
    {
        if (id != vehicleModel.Id) return BadRequest();
        var vehicleModelDto = await _appBLL.VehicleModels.FirstOrDefaultAsync(id);
        try
        {
            if (vehicleModelDto != null)
            {
                vehicleModelDto.VehicleMarkId = vehicleModel.VehicleMarkId;
                vehicleModelDto.VehicleModelName = vehicleModel.VehicleModelName;
                vehicleModelDto.UpdatedBy = User.GettingUserEmail();
                vehicleModelDto.CreatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.VehicleModels.Update(vehicleModelDto);
            }

            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleModelExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/VehicleModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new vehicle model
    /// </summary>
    /// <param name="vehicleModel">Vehicle model with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(VehicleModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleModel>> PostVehicleModel([FromBody]VehicleModel vehicleModel)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var vehicleModelDto = new VehicleModelDTO();
        vehicleModelDto.Id = Guid.NewGuid();
        vehicleModel.VehicleMarkId = vehicleModel.VehicleMarkId;
        vehicleModelDto.VehicleModelName = vehicleModel.VehicleModelName;
        vehicleModelDto.CreatedBy = User.GettingUserEmail();
        vehicleModelDto.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicleModelDto.UpdatedBy = User.GettingUserEmail();
        vehicleModelDto.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.VehicleModels.Add(vehicleModelDto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleModel", new
        {
            id = vehicleModel.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, vehicleModel);
    }

    // DELETE: api/VehicleModels/5
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
    public async Task<IActionResult> DeleteVehicleModel(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id);
        if (vehicleModel == null) return NotFound();

        await _appBLL.VehicleModels.RemoveAsync(vehicleModel.Id);
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
    private bool VehicleModelExists(Guid id)
    {
        return _appBLL.VehicleModels.Exists(id);
    }
}