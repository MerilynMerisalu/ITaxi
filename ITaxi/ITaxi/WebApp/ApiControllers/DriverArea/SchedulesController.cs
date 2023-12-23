#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.DriverArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;
/// <summary>
/// Api controller for schedules
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/DriverArea/[controller]")]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SchedulesController : ControllerBase

{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for schedules api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.DriverArea.ScheduleDTO to Public.DTO.v1.DriverArea.Schedule</param>
    public SchedulesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Schedules
    /// <summary>
    /// Gets all the schedules
    /// </summary>
    /// <returns>List of schedules with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Schedule>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        
        return Ok(res.Select(s => _mapper.Map<Schedule>(s)));
    }

    // GET: api/Schedules/5
    /// <summary>
    /// Returns schedule based on id
    /// </summary>
    /// <param name="id">Schedule id, Guid</param>
    /// <returns>Schedule (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Schedule), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Schedule>> GetSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        
        if (schedule == null) return NotFound();
       
        return Ok(_mapper.Map<Schedule>(schedule));
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an schedule
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="schedule">Entity which is updated</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutSchedule(Guid id, Schedule schedule)
    {
        var scheduleDTO = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id);
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var driver = await _appBLL.Drivers.GettingDriverByAppUserIdAsync(schedule.Driver!.AppUserId);
        
        try
        { 
        
            if (scheduleDTO != null && (roleName != "Admin" && scheduleDTO.Driver!.AppUserId != userId))
            {
                return Forbid();
            }

            if (scheduleDTO != null)
            {
                scheduleDTO.DriverId = driver.Id;
                scheduleDTO.VehicleId = schedule.VehicleId;
                scheduleDTO.StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime();
                scheduleDTO.EndDateAndTime = schedule.EndDateAndTime.ToUniversalTime();
                scheduleDTO.CreatedBy = User.GettingUserEmail();
                scheduleDTO.CreatedAt = DateTime.Now.ToUniversalTime();
                scheduleDTO.UpdatedBy = User.GettingUserEmail();
                scheduleDTO.UpdatedAt = DateTime.Now.ToUniversalTime();

                if (schedule != null) _appBLL.Schedules.Update(scheduleDTO);
            }

            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }

        return NoContent();
    }

    // POST: api/Schedules
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new schedule
    /// </summary>
    /// <param name="schedule">Schedule with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Schedule), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Schedule>> PostSchedule([FromBody]Schedule schedule)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var driverId =  _appBLL.Drivers.GettingDriverByAppUserIdAsync(userId).Result.Id;
        var scheduleDTO = new ScheduleDTO()
        {
            Id = Guid.NewGuid(),
            DriverId = driverId,
            VehicleId = schedule.VehicleId,
            StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime(),
            EndDateAndTime = schedule.EndDateAndTime.ToUniversalTime(),
            CreatedBy = User.GettingUserEmail(),
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedBy = User.GettingUserEmail(),
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Schedules.Add(scheduleDTO);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new
        {
            id = schedule.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, schedule);
    }
    

    // DELETE: api/Schedules/5
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        if (await _appBLL.RideTimes.HasScheduleAnyAsync(id) || await _appBLL.Bookings.HasAnyScheduleAsync(id))
            return BadRequest("Schedule cannot be deleted!");
        if (schedule == null) return NotFound();

        await _appBLL.Schedules.RemoveAsync(schedule.Id);
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
    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}