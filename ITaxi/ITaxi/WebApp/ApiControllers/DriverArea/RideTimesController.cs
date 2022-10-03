#nullable enable
using App.Contracts.DAL;
using App.Domain;
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
        return Ok(await _uow.RideTimes.GettingAllRideTimesWithoutIncludesAsync());
    }

    // GET: api/RideTimes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RideTime>> GetRideTime(Guid id)
    {
        var rideTime = await _uow.RideTimes.GettingRideTimeWithoutIncludesByIdAsync(id);

        if (rideTime == null) return NotFound();

        return rideTime;
    }

    // PUT: api/RideTimes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRideTime(Guid id, RideTime rideTime)
    {
        if (id != rideTime.Id) return BadRequest();


        try
        {
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
        _uow.RideTimes.Add(rideTime);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetRideTime", new {id = rideTime.Id}, rideTime);
    }

    // DELETE: api/RideTimes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRideTime(Guid id)
    {
        var rideTime = await _uow.RideTimes.GettingRideTimeWithoutIncludesByIdAsync(id);
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