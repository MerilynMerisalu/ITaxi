/*#nullable enable
using App.Contracts.DAL;
using App.Domain;
using App.Domain.DTO;
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
    private readonly IAppUnitOfWork _uow;
    public BookingsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Bookings.GettingAllOrderedBookingsAsync(userId, roleName);
        foreach (var booking in res)
        {
            if (booking != null)
            {
                booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime();
                booking.Schedule!.StartDateAndTime = booking.Schedule.StartDateAndTime.ToLocalTime();
                booking.Schedule!.EndDateAndTime = booking.Schedule.EndDateAndTime.ToLocalTime();
                booking.DeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime();
            }
        }
        return Ok(res);
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id, userId, roleName);

        if (booking == null) return NotFound();
        
        booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime();
        booking.Schedule!.StartDateAndTime = booking.Schedule.StartDateAndTime.ToLocalTime();
        booking.Schedule!.EndDateAndTime = booking.Schedule.EndDateAndTime.ToLocalTime();
        booking.DeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime();
        return booking;
    }

    // PUT: api/Bookings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(Guid id, Booking? booking)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        booking = await _uow.Bookings.GettingBookingAsync(id, userId, roleName);
        if (booking == null)
        {
            return NotFound();
        }
        var drive = await _uow.Drives.SingleOrDefaultAsync(d => d!.Booking!.DriveId.Equals(booking.DriverId));


        try
        { booking.UpdatedBy = User.Identity!.Name!;
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.Bookings.Update(booking);
            if (drive != null) _uow.Drives.Update(drive);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Bookings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(Booking booking)
    {
        var userId = User.GettingUserId();
        if (booking.Customer!.AppUserId != userId)
        {
            return Forbid();
        }
        booking.Customer.AppUserId = userId;
        booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToUniversalTime();
        booking.CreatedBy = User.Identity!.Name;
        booking.CreatedAt = DateTime.Now.ToUniversalTime();
        booking.UpdatedBy = User.Identity!.Name;
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        _uow.Bookings.Add(booking);
#warning Needs checking
        var drive = new Drive
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            Booking = booking,
            CreatedBy = User.Identity.Name,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedBy = User.Identity.Name,
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _uow.Drives.Add(drive);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetBooking", new {id = booking.Id}, booking);
    }

    // DELETE: api/Bookings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
#warning Needs checking
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id, userId, roleName);
        if (booking == null) return NotFound();

        var drive = await _uow.Drives.GettingSingleOrDefaultDriveAsync(d => d!.Booking!.DriverId.Equals(booking.DriveId)
        );

        if (drive != null) await _uow.Drives.RemoveAsync(drive.Id);
        _uow.Bookings.Remove(booking);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingExists(Guid id)
    {
        return _uow.Bookings.Exists(id);
    }
}*/