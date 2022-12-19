/*#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;

[Route("api/DriverArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SchedulesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public SchedulesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Schedules
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        foreach (var schedule in res)
        {
            schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
            schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
            schedule.CreatedAt = schedule.CreatedAt.ToLocalTime();
            schedule.UpdatedAt = schedule.UpdatedAt.ToLocalTime();
        }
        return Ok(res);
    }

    // GET: api/Schedules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Schedule>> GetSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        
        if (schedule == null) return NotFound();
        schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
        schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
        schedule.CreatedAt = schedule.CreatedAt.ToLocalTime();
        schedule.UpdatedAt = schedule.UpdatedAt.ToLocalTime();

        return schedule;
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(Guid id, Schedule schedule)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        
        try
        {
            if (userId != schedule.Driver!.AppUserId || !roleName.Equals("Admin"))
            {
                return NotFound();
            }
            schedule.StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime();
            schedule.EndDateAndTime = schedule.EndDateAndTime.ToUniversalTime();
            schedule.UpdatedAt = schedule.UpdatedAt.ToUniversalTime();
            _uow.Schedules.Update(schedule);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }

        return NoContent();
    }

    // POST: api/Schedules
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != nameof(Admin) || schedule.Driver!.AppUserId != userId)
        {
            return Forbid();
        }
        schedule.Driver!.AppUserId = userId;
        schedule.StartDateAndTime = schedule.StartDateAndTime.ToUniversalTime();
        schedule.StartDateAndTime = schedule.EndDateAndTime.ToUniversalTime();
        schedule.CreatedBy = User.Identity!.Name;
        schedule.CreatedAt = DateTime.Now.ToUniversalTime();
        schedule.UpdatedBy = User.Identity!.Name;
        schedule.UpdatedAt = DateTime.Now.ToLocalTime();
        _uow.Schedules.Add(schedule);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new {id = schedule.Id}, schedule);
    }

    // DELETE: api/Schedules/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        if (schedule == null) return NotFound();

        _uow.Schedules.Remove(schedule);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    /*private bool ScheduleExists(Guid id)
    {
        return _uow.Schedules.Exists(id);
    }#1#
}*/