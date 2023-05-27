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
/// Api controller for ride times
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RideTimesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for ride times api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.RideTimeDTO to Public.DTO.v1.AdminArea.RideTime</param>
    public RideTimesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/RideTimes
    /// <summary>
    /// Gets all the ride times
    /// </summary>
    /// <returns>List of ride times with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<RideTime>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<RideTime>>> GetRideTimes()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.RideTimes.GettingAllOrderedRideTimesAsync(userId, roleName);
        
        return Ok(res.Select(r=> _mapper.Map<RideTime>(r)));
    }

    // GET: api/RideTimes/5
    /// <summary>
    /// Returns ride time based on id
    /// </summary>
    /// <param name="id">Ride time id, Guid</param>
    /// <returns>Ride time (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RideTime), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RideTime>> GetRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);

        if (rideTime == null) return NotFound();
        
        return _mapper.Map<RideTime>(rideTime);
    }

    // PUT: api/RideTimes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a ride time
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="rideTime">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutRideTime(Guid id, RideTime rideTime)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTimeDTO = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        
        if (rideTimeDTO == null)
        {
            return NotFound();
        }
        
        try
        {
            rideTimeDTO.ScheduleId = rideTime.ScheduleId;
            rideTimeDTO.RideDateTime = rideTime.RideDateTime.ToUniversalTime();
            rideTimeDTO.IsTaken = rideTime.IsTaken;
            rideTimeDTO.UpdatedBy = User.GettingUserEmail();
            rideTimeDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
            rideTime.UpdatedBy = User.Identity!.Name;
            
            _appBLL.RideTimes.Update(rideTimeDTO);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RideTimeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/RideTimes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new ride time
    /// </summary>
    /// <param name="rideTime">Ride time with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(RideTime), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RideTime>> PostRideTime([FromBody]RideTime rideTime)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        rideTime.Id = Guid.NewGuid();
        rideTime.RideDateTime = rideTime.RideDateTime.ToUniversalTime();
        rideTime.CreatedBy = User.GettingUserEmail();
        rideTime.CreatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.RideTimes.Add(_mapper.Map<RideTimeDTO>(rideTime));
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetRideTime", new
        {
            id = rideTime.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, rideTime);
    }

    // DELETE: api/RideTimes/5
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
    public async Task<IActionResult> DeleteRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        if (rideTime == null) return NotFound();

        await _appBLL.RideTimes.RemoveAsync(rideTime.Id);
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
    private bool RideTimeExists(Guid id)
    {
        return _appBLL.RideTimes.Exists(id);
    }
}