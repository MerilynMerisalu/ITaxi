#nullable enable

using App.Contracts.BLL;
using App.Public.DTO.v1.DriverArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.DriverArea;

/// <summary>
/// Api controller for drives
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/DriverArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DrivesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructor for drives api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.DriverArea.DriveDTO to Public.DTO.v1.DriverArea.Drive</param>
    public DrivesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Drives
    /// <summary>
    /// Gets all the drives
    /// </summary>
    /// <returns>List of drives with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Drive>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Drive>>> GetDrives()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Drives.GettingAllOrderedDrivesWithIncludesAsync(userId, roleName);
        var results = res.Select(d => _mapper.Map<Drive>(d));
        return Ok(results);
    }

    // GET: api/Drives/5
    /// <summary>
    /// Returns drive based on id
    /// </summary>
    /// <param name="id">Drive id, Guid</param>
    /// <returns>Drive (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Drive), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Drive>> GetDrive(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var drive = await _appBLL.Drives.GettingFirstDriveAsync(id, userId, roleName);

        if (drive == null) return NotFound();
        
        return Ok(_mapper.Map<Drive>(drive));
    }
    
    /// <summary>
    /// Accepting booking by the driver
    /// </summary>
    /// <param name="id">Drive id</param>
    /// <returns>Returns 204</returns>
    [HttpGet("Accept/{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Drive), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Accept (Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var drive = await _appBLL.Drives.GettingFirstDriveAsync(id, userId, roleName, false);
        if (drive != null)
        {
           drive = await _appBLL.Drives.AcceptingDriveAsync(drive.Id, userId, roleName);
        }

        return NoContent();
    }
    
    /// <summary>
    /// Checks if the ride exists
    /// </summary>
    /// <param name="id">Drive id</param>
    /// <returns>Boolean</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Drive), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool DriveExists(Guid id)
    {
        return _appBLL.Drives.Exists(id);
    }
    
}