#nullable enable
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for drives
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DrivesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for drives api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.DriveDTO to Public.DTO.v1.AdminArea.Drive</param>
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Drive>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Drive>>> GetDrives()
    {
        var res = await _appBLL.Drives.GetAllDrivesWithoutIncludesAsync();
        return Ok(res.Select(d=> _mapper.Map<Drive>(d)));
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
    [ProducesResponseType(typeof(County), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Drive>> GetDrive(Guid id)
    {
        var drive = await _appBLL.Drives.GettingDriveWithoutIncludesAsync(id);

        if (drive == null) return NotFound();

        return _mapper.Map<Drive>(drive);
    }
    
}