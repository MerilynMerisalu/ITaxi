#nullable enable
using App.BLL;
using App.BLL.DTO.AdminArea;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SchedulesController : ControllerBase
{
    private readonly AppBLL _appBLL;

    public SchedulesController(AppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Schedules
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
    {
        return Ok(await _appBLL.Schedules.GettingAllOrderedSchedulesWithoutIncludesAsync());
    }

    // GET: api/Schedules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDTO>> GetSchedule(Guid id)
    {
        var schedule = await _appBLL.Schedules.GettingScheduleWithoutIncludesAsync(id);

        if (schedule == null) return NotFound();

        return schedule;
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(Guid id, ScheduleDTO schedule)
    {
        if (id != schedule.Id) return BadRequest();
        
        try
        {
            _appBLL.Schedules.Update(schedule);
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
    [HttpPost]
    public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO schedule)
    {
        _appBLL.Schedules.Add(schedule);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new {id = schedule.Id}, schedule);
    }

    // DELETE: api/Schedules/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var schedule = await _appBLL.Schedules.GettingScheduleWithoutIncludesAsync(id);
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