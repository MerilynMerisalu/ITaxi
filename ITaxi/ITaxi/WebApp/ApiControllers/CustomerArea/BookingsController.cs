#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using App.Public.DTO.v1.CustomerArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.CustomerArea;

/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/CustomerArea/[controller]")]
[Authorize(Roles = "Admin, Customer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appBLL"></param>
    /// <param name="mapper"></param>
    public BookingsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Bookings
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Booking>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(userId, roleName);
        
        return Ok(res.Select(b => _mapper.Map<Booking>(b)));
    }

    // GET: api/Bookings/5
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Booking>> GetBooking(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName);

        if (booking == null) return NotFound();
        
        booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime();
        booking.Schedule!.StartDateAndTime = booking.Schedule.StartDateAndTime.ToLocalTime();
        booking.Schedule!.EndDateAndTime = booking.Schedule.EndDateAndTime.ToLocalTime();
        booking.DeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime();
        return Ok(_mapper.Map<Booking>(booking));
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
    [Produces("application/json")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Booking>> PostBooking([FromBody]Booking booking)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        
        var userId = User.GettingUserId();
       
        // We need to resolve the following fields first:
        // DriverId
        // ScheduleId
        // VehicleId,
        // DriveId -- set after booking
        var rideTimes = await _appBLL.RideTimes.GettingBestAvailableRideTimeAsync(booking.PickUpDateAndTime,
            booking.CityId, booking.NumberOfPassengers, booking.VehicleTypeId, false, userId, roleName: null);
        if (rideTimes == null || rideTimes.Count == 0)
        {
            return BadRequest();
        }
        else if (rideTimes.Count > 1)
        {
            return Conflict();
        }

        var rideTime = rideTimes.First();

        var bookingDTO = new BookingDTO();
        bookingDTO.Id = Guid.NewGuid();
        bookingDTO.CityId = booking.CityId;
        bookingDTO.DriverId = rideTime.DriverId;
        bookingDTO.CustomerId = _appBLL.Customers.GettingCustomerIdByAppUserIdAsync(userId).Result;
        bookingDTO.ScheduleId = rideTime.ScheduleId;
        bookingDTO.VehicleId = rideTime.Schedule!.VehicleId;
        bookingDTO.DestinationAddress = booking.DestinationAddress;
        bookingDTO.AdditionalInfo = booking.AdditionalInfo;
        bookingDTO.PickupAddress = booking.PickupAddress;
        bookingDTO.HasAnAssistant = booking.HasAnAssistant;
        bookingDTO.NumberOfPassengers = booking.NumberOfPassengers;
        // Requires Drive to be created first
        //bookingDTO.DriveId = booking.DriveId;
        bookingDTO.VehicleTypeId = booking.VehicleTypeId;
        bookingDTO.StatusOfBooking = StatusOfBooking.Awaiting;
        bookingDTO.PickUpDateAndTime = booking.PickUpDateAndTime.ToUniversalTime();
        bookingDTO.CreatedBy = User.GettingUserEmail();
        bookingDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        bookingDTO.UpdatedBy = User.GettingUserEmail();
        bookingDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        _appBLL.Bookings.Add(bookingDTO);
#warning Needs checking
        var drive = new DriveDTO()
        {
            Id = new Guid(),
            DriverId = bookingDTO.DriverId,
            Booking = bookingDTO,
            CreatedBy = User.Identity!.Name,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedBy = User.Identity.Name,
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _appBLL.Drives.Add(drive);
        await _appBLL.SaveChangesAsync();
        
        return CreatedAtAction("GetBooking", new
        {
            id = booking.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, booking);
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
        await _appBLL.Bookings.RemoveAsync(booking.Id);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingExists(Guid id)
    {
        return _appBLL.Bookings.Exists(id);
    }
}