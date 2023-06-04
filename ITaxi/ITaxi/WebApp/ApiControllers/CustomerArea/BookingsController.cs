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
/// Api controller for customer area bookings
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
    /// Constructor for customer area bookings api controller
    /// </summary>
    /// <param name="appBLL"></param>
    /// <param name="mapper"></param>
    public BookingsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Bookings
    /// <summary>
    /// Gets all the bookings
    /// </summary>
    /// <returns>List of bookings with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Booking>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(userId, roleName);
        
        return Ok(res.Select(b => _mapper.Map<Booking>(b)));
    }

    // GET: api/Bookings/5
    /// <summary>
    /// Returns booking based on id
    /// </summary>
    /// <param name="id">Booking id, Guid</param>
    /// <returns>Booking (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

   
    // POST: api/Bookings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new booking
    /// </summary>
    /// <param name="booking">Booking with properties</param>
    /// <returns>Status201Created with an entity</returns>
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
        bookingDTO.VehicleTypeId = booking.VehicleTypeId;
        bookingDTO.StatusOfBooking = StatusOfBooking.Awaiting;
        bookingDTO.PickUpDateAndTime = booking.PickUpDateAndTime.ToUniversalTime();
        bookingDTO.CreatedBy = User.GettingUserEmail();
        bookingDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        bookingDTO.UpdatedBy = User.GettingUserEmail();
        bookingDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        _appBLL.Bookings.Add(bookingDTO);
        
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
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
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

    /// <summary>
    /// User can decline a booking
    /// </summary>
    /// <param name="id">Booking id</param>
    /// <returns>Returns 204</returns>
    [HttpGet("Decline/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    
    public async Task<IActionResult> Decline(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName, false);
        if (booking != null)
        {
            await _appBLL.Bookings.BookingDeclineAsync(booking.Id, userId, roleName);
            
        }

        return NoContent();
    }
    
    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool BookingExists(Guid id)
    {
        return _appBLL.Bookings.Exists(id);
    }
}