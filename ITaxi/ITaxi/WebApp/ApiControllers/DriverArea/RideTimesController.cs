#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/DriverArea/[controller]")]
[ApiController]
public class RideTimesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public RideTimesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/RideTimes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RideTime>>> GetRideTimes()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.RideTimes.GettingAllOrderedRideTimesAsync(userId, roleName);
        foreach (var rideTime in res)
        {
            if (rideTime != null)
            {
                rideTime.Schedule!.StartDateAndTime = rideTime.Schedule!.StartDateAndTime.ToLocalTime();
                rideTime.Schedule!.EndDateAndTime = rideTime.Schedule!.EndDateAndTime.ToLocalTime();
                rideTime.RideDateTime = rideTime.RideDateTime.ToLocalTime();
                rideTime.CreatedAt = rideTime.CreatedAt.ToLocalTime();
                rideTime.UpdatedAt = rideTime.UpdatedAt.ToLocalTime();
            }
        }
        return Ok(res);
    }

    // GET: api/RideTimes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RideTime>> GetRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);

        if (rideTime == null) return NotFound();
        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule!.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule!.EndDateAndTime.ToLocalTime();
        rideTime.RideDateTime = rideTime.RideDateTime.ToLocalTime();
        rideTime.CreatedAt = rideTime.CreatedAt.ToLocalTime();
        rideTime.UpdatedAt = rideTime.UpdatedAt.ToLocalTime();

        return rideTime;
    }

    // PUT: api/RideTimes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRideTime(Guid id, RideTime? rideTime)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        
        if (rideTime == null)
        {
            return NotFound();
        }


        try
        {
            rideTime.RideDateTime = rideTime.RideDateTime.ToUniversalTime();
            rideTime.UpdatedBy = User.Identity!.Name;
            rideTime.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.RideTimes.Update(rideTime);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<RideTime>> PostRideTime(RideTime rideTime)
    {
        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToUniversalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToUniversalTime();
        rideTime.RideDateTime = rideTime.RideDateTime.ToUniversalTime();
        rideTime.CreatedBy = User.Identity!.Name;
        rideTime.CreatedAt = DateTime.Now.ToUniversalTime();
        rideTime.UpdatedBy = User.Identity!.Name;
        rideTime.UpdatedAt = DateTime.Now.ToUniversalTime();
        _uow.RideTimes.Add(rideTime);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetRideTime", new {id = rideTime.Id}, rideTime);
    }

    // DELETE: api/RideTimes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        if (rideTime == null) return NotFound();

        _uow.RideTimes.Remove(rideTime);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool RideTimeExists(Guid id)
    {
        return _uow.RideTimes.Exists(id);
    }
}