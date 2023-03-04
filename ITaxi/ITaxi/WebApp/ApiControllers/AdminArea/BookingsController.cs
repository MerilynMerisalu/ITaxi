#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingsController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public BookingsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
    {
        return Ok(await _appBLL.Bookings.GettingAllOrderedBookingsWithoutIncludesAsync());
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDTO>> GetBooking(Guid id)
    {
        var booking = await _appBLL.Bookings.GettingBookingWithoutIncludesByIdAsync(id);

        if (booking == null) return NotFound();

        return booking;
    }

    // PUT: api/Bookings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    /*public async Task<IActionResult> PutBooking(Guid id, BookingDTO booking)
    {*/
        /*if (id != booking.Id) return BadRequest();

        var drive = await _appBLL.Drives.FirstOrDefaultAsync(id);


        try
        {
            _appBLL.Bookings.Update(booking);
            if (drive != null) _appBLL.Drives.Update(drive);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
    */

    // POST: api/Bookings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BookingDTO>> PostBooking([FromBody] BookingDTO booking)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Bookings.Add(booking);
#warning Needs checking
        var drive = new DriveDTO()
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            Booking = booking
        };
        _appBLL.Drives.Add(drive);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetBooking", new
        {
            id = booking.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, booking);
    }

    // DELETE: api/Bookings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
#warning Needs checking
        var booking = await _appBLL.Bookings.GettingBookingWithoutIncludesByIdAsync(id);
        if (booking == null) return NotFound();

        var drive = await _appBLL.Drives.GettingDriveByBookingIdAsync(booking.Id, noTracking:true, noIncludes:true);

        if (drive != null) await _appBLL.Drives.RemoveAsync(drive.Id);
        _appBLL.Bookings.Remove(booking);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingExists(Guid id)
    {
        return _appBLL.Bookings.Exists(id);
    }
}