#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RideTimesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public RideTimesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/RideTimes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RideTimeDTO>>> GetRideTimes()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.RideTimes.GettingAllOrderedRideTimesAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/RideTimes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RideTimeDTO>> GetRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);

        if (rideTime == null) return NotFound();
        

        return rideTime;
    }

    // PUT: api/RideTimes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRideTime(Guid id, RideTimeDTO? rideTime)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        
        if (rideTime == null)
        {
            return NotFound();
        }


        try
        {
            
            rideTime.UpdatedBy = User.Identity!.Name;
            
            _appBLL.RideTimes.Update(rideTime);
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
    [HttpPost]
    public async Task<ActionResult<RideTimeDTO>> PostRideTime([FromBody]RideTimeDTO rideTime)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToUniversalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToUniversalTime();
        rideTime.RideDateTime = rideTime.RideDateTime.ToUniversalTime();
        rideTime.CreatedBy = User.Identity!.Name;
        rideTime.CreatedAt = DateTime.Now.ToUniversalTime();
        rideTime.UpdatedBy = User.Identity!.Name;
        rideTime.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.RideTimes.Add(rideTime);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetRideTime", new
        {
            id = rideTime.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, rideTime);
    }

    // DELETE: api/RideTimes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRideTime(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);
        if (rideTime == null) return NotFound();

        _appBLL.RideTimes.Remove(rideTime);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool RideTimeExists(Guid id)
    {
        return _appBLL.RideTimes.Exists(id);
    }
}