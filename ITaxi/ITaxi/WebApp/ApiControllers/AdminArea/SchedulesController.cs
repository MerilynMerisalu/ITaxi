#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = nameof(Admin), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        return Ok(await _uow.Schedules.GettingAllOrderedSchedulesWithoutIncludesAsync());
    }

    // GET: api/Schedules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Schedule>> GetSchedule(Guid id)
    {
        var schedule = await _uow.Schedules.GettingScheduleWithoutIncludesAsync(id);

        if (schedule == null) return NotFound();

        return schedule;
    }

    // PUT: api/Schedules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(Guid id, Schedule schedule)
    {
        if (id != schedule.Id) return BadRequest();


        try
        {
            _uow.Schedules.Update(schedule);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
    {
        _uow.Schedules.Add(schedule);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetSchedule", new {id = schedule.Id}, schedule);
    }

    // DELETE: api/Schedules/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var schedule = await _uow.Schedules.GettingScheduleWithoutIncludesAsync(id);
        if (schedule == null) return NotFound();

        _uow.Schedules.Remove(schedule);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool ScheduleExists(Guid id)
    {
        return _uow.Schedules.Exists(id);
    }
}