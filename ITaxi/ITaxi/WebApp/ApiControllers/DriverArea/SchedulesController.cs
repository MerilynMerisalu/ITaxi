#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.DAL;

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
    private readonly IAppUnitOfWork _appBLL;

    public SchedulesController(IAppUnitOfWork uow)
    {
        _appBLL = uow;
    }

    // GET: api/Schedules
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/Schedules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDTO>> GetSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        
        if (schedule == null) return NotFound();
        schedule.StartDateAndTime = schedule.StartDateAndTime;
        schedule.EndDateAndTime = schedule.EndDateAndTime;
        schedule.CreatedAt = schedule.CreatedAt;
        schedule.UpdatedAt = schedule.UpdatedAt;

        return Ok(schedule);
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(Guid id)
    {
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id);
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        
        try
        {
            if (userId != schedule.Driver!.AppUserId || !roleName.Equals("Admin"))
            {
                return NotFound();
            }
            
            _appBLL.Schedules.Update(schedule);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }

        return NoContent();
    }

    // POST: api/Schedules
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    /*public async Task<ActionResult<ScheduleDTO>> PostSchedule(App.BLL.DTO.AdminArea.ScheduleDTO schedule)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != "Admin" || schedule.Driver!.AppUserId != userId)
        {
            return Forbid();
        }
        schedule.Driver!.AppUserId = userId;
        _appBLL.Schedules.Add(schedule);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new {id = schedule.Id}, schedule);
    }
    */

    // DELETE: api/Schedules/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        if (schedule == null) return NotFound();

        _appBLL.Schedules.Remove(schedule);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}