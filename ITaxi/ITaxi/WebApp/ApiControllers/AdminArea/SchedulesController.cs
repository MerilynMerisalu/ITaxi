#nullable enable
using App.BLL;
using App.BLL.DTO.AdminArea;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for schedules
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SchedulesController : ControllerBase
{
    private readonly AppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for schedules api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.ScheduleDTO to Public.DTO.v1.AdminArea.Schedule</param>
    public SchedulesController(AppBLL appBLL, IMapper mapper)
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
        var res = await _appBLL.Schedules.GettingAllOrderedSchedulesWithoutIncludesAsync();
        return Ok(res.Select(s=> _mapper.Map<Schedule>(s)));
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
        var schedule = await _appBLL.Schedules.GettingScheduleWithoutIncludesAsync(id);

        if (schedule == null) return NotFound();

        return _mapper.Map<Schedule>(schedule);
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an schedule
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="schedule">DTO which holds the values</param>
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
        if (id != schedule.Id) return BadRequest();

        var sheduleDTO = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id);
        try
        {
            if (sheduleDTO != null)
            {
                sheduleDTO.VehicleId = schedule.VehicleId;
                sheduleDTO.StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime();
                sheduleDTO.EndDateAndTime = schedule.EndDateAndTime.ToUniversalTime();
                sheduleDTO.UpdatedBy = User.GettingUserEmail();
                sheduleDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Schedules.Update(sheduleDTO);
            }

            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ScheduleExists(id))
                return NotFound();
            throw;
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
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var scheduleDTO = new ScheduleDTO();
        scheduleDTO.Id = Guid.NewGuid();
        scheduleDTO.VehicleId = schedule.VehicleId;
        scheduleDTO.StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime();
        scheduleDTO.EndDateAndTime = schedule.EndDateAndTime.ToUniversalTime();
        scheduleDTO.CreatedBy = User.GettingUserEmail();
        scheduleDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Schedules.Add(scheduleDTO);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new
        {
            id = schedule.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
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
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var schedule = await _appBLL.Schedules.GettingScheduleWithoutIncludesAsync(id);
        if (schedule == null) return NotFound();

        _appBLL.Schedules.Remove(schedule);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}