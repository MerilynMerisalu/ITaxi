#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for admin area bookings
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for admin area bookings api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.BookingDTO to Public.DTO.v1.AdminArea.Booking</param>
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
        var res = await _appBLL.Bookings.GettingAllOrderedBookingsWithoutIncludesAsync();
        return Ok(res.Select(b=> _mapper.Map<Booking>(b)));
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
        var booking = await _appBLL.Bookings.GettingBookingWithoutIncludesByIdAsync(id);

        if (booking == null) return NotFound();

        return _mapper.Map<Booking>(booking);
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
    public async Task<ActionResult<Booking>> PostBooking([FromBody] Booking booking)
    {
        var bookingDTO = new BookingDTO();
        bookingDTO.Id = Guid.NewGuid();
        bookingDTO.CityId = booking.CityId;
        bookingDTO.DriverId = booking.DriverId;
        bookingDTO.CustomerId = booking.CustomerId;
        bookingDTO.ScheduleId = booking.ScheduleId;
        bookingDTO.VehicleId = booking.VehicleId;
        bookingDTO.DestinationAddress = booking.DestinationAddress;
        bookingDTO.AdditionalInfo = booking.AdditionalInfo;
        bookingDTO.PickupAddress = booking.PickupAddress;
        bookingDTO.HasAnAssistant = booking.HasAnAssistant;
        bookingDTO.NumberOfPassengers = booking.NumberOfPassengers;
        bookingDTO.DriveId = booking.DriveId;
        bookingDTO.VehicleTypeId = booking.VehicleTypeId;
        bookingDTO.StatusOfBooking = StatusOfBooking.Awaiting;
        bookingDTO.PickUpDateAndTime = booking.PickUpDateAndTime.ToUniversalTime();
        bookingDTO.CreatedBy = User.GettingUserEmail();
        bookingDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        bookingDTO.UpdatedBy = User.GettingUserEmail();
        bookingDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Bookings.Add(bookingDTO);
#warning Needs checking
        var drive = new DriveDTO()
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            Booking = bookingDTO
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
#warning Needs checking
        var booking = await _appBLL.Bookings.GettingBookingWithoutIncludesByIdAsync(id);
        if (booking == null) return NotFound();

        var drive = await _appBLL.Drives.GettingDriveByBookingIdAsync(booking.Id, noTracking:true, noIncludes:true);

        if (drive != null) await _appBLL.Drives.RemoveAsync(drive.Id);
        _appBLL.Bookings.Remove(booking);
        await _appBLL.SaveChangesAsync();

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