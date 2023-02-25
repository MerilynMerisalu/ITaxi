#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.CustomerArea;

[Route("api/CustomerArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Customer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDTO>> GetBooking(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName);

        if (booking == null) return NotFound();
        
        booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime();
        booking.Schedule!.StartDateAndTime = booking.Schedule.StartDateAndTime.ToLocalTime();
        booking.Schedule!.EndDateAndTime = booking.Schedule.EndDateAndTime.ToLocalTime();
        booking.DeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime();
        return Ok(booking);
    }

    // PUT: api/Bookings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(Guid id, Booking? booking)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName);
        if (booking == null)
        {
            return NotFound();
        }
        var drive = await _appBLL.Drives.SingleOrDefaultAsync(d => d!.Booking!.DriveId.Equals(booking.DriverId));


        try
        { booking.UpdatedBy = User.Identity!.Name!;
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
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
    public async Task<ActionResult<BookingDTO>> PostBooking(BookingDTO booking)
    {
        var userId = User.GettingUserId();
        if (booking.Customer!.AppUserId != userId)
        {
            return Forbid();
        }
        booking.Customer.AppUserId = userId;
        
        _appBLL.Bookings.Add(booking);
#warning Needs checking
        var drive = new DriveDTO()
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            Booking = booking,
            CreatedBy = User.Identity.Name,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedBy = User.Identity.Name,
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _appBLL.Drives.Add(drive);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetBooking", new {id = booking.Id}, booking);
    }

    // DELETE: api/Bookings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
#warning Needs checking
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName);
        if (booking == null) return NotFound();

        var drive = await _appBLL.Drives.GettingDriveByBookingIdAsync(booking.Id, userId, roleName, noIncludes:true, noTracking:true);

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