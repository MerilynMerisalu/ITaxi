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
        return Ok(await _uow.Bookings.GettingAllOrderedBookingsWithoutIncludesAsync());
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(Guid id)
    {
        var booking = await _uow.Bookings.GettingBookingWithoutIncludesByIdAsync(id);

        if (booking == null) return NotFound();

        return booking;
    }

    // PUT: api/Bookings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(Guid id, Booking booking)
    {
        if (id != booking.Id) return BadRequest();

        var drive = await _uow.Drives.SingleOrDefaultAsync(d => d!.Booking!.DriveId.Equals(booking.DriverId));


        try
        {
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
        _uow.Bookings.Add(booking);
#warning Needs checking
        var drive = new Drive
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            Booking = booking
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
        var booking = await _uow.Bookings.GettingBookingWithoutIncludesByIdAsync(id);
        if (booking == null) return NotFound();

        var drive = await _uow.Drives.SingleOrDefaultAsync(d => d!.Booking!.DriverId.Equals(booking.DriveId));

        if (drive != null) await _uow.Drives.RemoveAsync(drive.Id);
        _uow.Bookings.Remove(booking);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingExists(Guid id)
    {
        return _uow.Bookings.Exists(id);
    }
}