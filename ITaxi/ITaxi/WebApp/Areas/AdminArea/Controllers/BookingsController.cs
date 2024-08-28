#nullable enable
using System.Net;
using System.Net.Mail;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.AdminArea.ViewModels;
using WebApp.Helpers;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area booking controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class BookingsController : Controller
{
    private readonly IMailService _mailService;
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area booking controller constructor
    /// </summary>
    /// <param name="mailService"> Mail service</param>
    /// <param name="appBLL">AppBLL</param>
    public BookingsController(IMailService mailService, IAppBLL appBLL)
    {
        _mailService = mailService;
        _appBLL = appBLL;
    }
    
    // GET: AdminArea/Bookings
    /// <summary>
    /// Admin area bookings index
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();

        var res = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(null, roleName);
        return View(res);
    }

    // GET: AdminArea/Bookings/Details/5
    /// <summary>
    /// Admin area booking details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, null, roleName);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        vm.ShiftDurationTime = booking.Schedule!.ShiftDurationTime;
        vm.City = booking.City!.CityName;
        vm.Driver = booking.Driver!.AppUser!.LastAndFirstName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        vm.BookingDeclineDateAndTime = booking.DeclineDateAndTime.ToString("G");
        vm.IsDeclined = booking.IsDeclined;
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt;
        vm.UpdatedBy = booking.UpdatedBy!;
        vm.UpdatedAt = booking.UpdatedAt;

        return View(vm);
    }

    /// <summary>
    /// Booking set drop down list request
    /// </summary>
    public class BookingSetDropDownListRequest
    {
        /// <summary>
        /// List type
        /// </summary>
        public string? ListType { get; set; }
        
        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Vehicle type id
        /// </summary>
        public Guid VehicleTypeId { get; set; } = default!;
        
        /// <summary>
        /// City id
        /// </summary>
        public Guid CityId { get; set; } = default!;

        /// <summary>
        /// Number of passengers
        /// </summary>
        public int NumberOfPassengers { get; set; }

        /// <summary>
        /// Pickup date and time
        /// </summary>
        public DateTime PickupDateAndTime { get; set; }
        
        /// <summary>
        /// Ride time id
        /// </summary>
        public Guid? RideTimeId { get; set; }
    }

    /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    /// <param name="parameters">Parameters</param>
    /// <returns>View model</returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody] BookingSetDropDownListRequest parameters)
    {
        // Use the EditRideTimeViewModel because I want to send through the SelectLists and Ids that have now changed
        var vm = new CreateBookingViewModel();
        //IEnumerable<ScheduleDTO>? schedules = null;
        //Guid id = Guid.Parse(value);

        if (parameters.ListType == nameof(BookingDTO.PickUpDateAndTime))
        {
            // If the UI provides a RideTimeId, then I need to clear or release this time first
            if (parameters.RideTimeId.HasValue && parameters.RideTimeId != Guid.Empty)
            {
                var rideTime =
                    await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(parameters.RideTimeId.Value, null, null, false);
                if (rideTime != null)
                {
                    rideTime.ExpiryTime = null;
                    await _appBLL.SaveChangesAsync();
                }
            }

            // Value in this case IS the pickupDateAndTime
            parameters.PickupDateAndTime = DateTime.Parse(parameters.Value!);
            if (parameters.PickupDateAndTime == DateTime.MinValue)
            {
                // Do nothing, this will show the message to re-select the time entry
            }
            else
            {
                parameters.PickupDateAndTime = parameters.PickupDateAndTime.ToUniversalTime();

                // Want to find the nearest available ride date and time
                // First I need get a list of the available ride times
                var bestTimes = await _appBLL.RideTimes.GettingBestAvailableRideTimeAsync(parameters.PickupDateAndTime,
                    parameters.CityId, parameters.NumberOfPassengers, parameters.VehicleTypeId, true);
                if (bestTimes.Any())
                {
                    if (bestTimes.Count == 1)
                    {
                        var bestTime = bestTimes.First();
                        bestTime.Schedule!.StartDateAndTime = bestTime.Schedule.StartDateAndTime.ToLocalTime();
                        bestTime.Schedule.EndDateAndTime = bestTime.Schedule.EndDateAndTime.ToLocalTime();

                        vm.RideTimeId = bestTime.Id;
                        vm.Schedules = new SelectList(new[] {bestTime.Schedule}, nameof(ScheduleDTO.Id),
                            nameof(ScheduleDTO.ShiftDurationTime));
                        vm.ScheduleId = bestTime.ScheduleId;
                        vm.Drivers = new SelectList(new[] {bestTime.Schedule.Driver}, nameof(DriverDTO.Id),
                            "AppUser.LastAndFirstName");
                        vm.DriverId = bestTime.DriverId;
                        vm.Vehicles = new SelectList(new[] {bestTime.Schedule!.Vehicle}, nameof(VehicleDTO.Id),
                            nameof(VehicleDTO.VehicleIdentifier));
                        vm.VehicleId = bestTime.Schedule!.VehicleId;
                    }
                    else
                    {
                        var min = DateTime.UtcNow.AddHours(-24);
                        var max = parameters.PickupDateAndTime.AddHours(24);
                        var timesForDisplay = bestTimes
                            .Where(x => x.RideDateTime > min && x.RideDateTime < max)
                            .Select(x => new
                            {
                                Value = x.RideDateTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm"),
                                Text = x.RideDateTime.ToLocalTime().ToString("g")
                            })
                            .ToList();
                        if (timesForDisplay.Any())
                            vm.RideTimes = new SelectList(timesForDisplay, "Value", "Text");
                    }
                }
            }
        }

        // This is not currently in use, this is theoretically what I would do if the user was
        // forced to select a specific RideTime, I have changed this and instead
        // the user is forced to enter a Pickup Date Time that matches an existing Ride Time
        if (parameters.ListType == "RideTimeId")
        {
            var rideTimeId = Guid.Parse(parameters.Value!);
            var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(rideTimeId);

            vm.RideTimeId = rideTimeId;
            vm.Schedules = new SelectList(new[] {rideTime!.Schedule}, nameof(ScheduleDTO.Id),
                nameof(ScheduleDTO.ShiftDurationTime));
            vm.ScheduleId = rideTime.ScheduleId;
            vm.Drivers = new SelectList(new[] {rideTime.Driver}, nameof(DriverDTO.Id),
                "AppUser.LastAndFirstName");
            vm.DriverId = rideTime.DriverId;
            vm.Vehicles = new SelectList(new[] {rideTime.Schedule!.Vehicle}, nameof(VehicleDTO.Id),
                nameof(VehicleDTO.VehicleIdentifier));
            vm.VehicleId = rideTime.Schedule!.VehicleId;
        }

        return Ok(vm);
    }

    // GET: AdminArea/Bookings/Create
    /// <summary>
    /// Admin area booking create GET method
    /// </summary>
    /// <returns>View model</returns>
    public async Task<IActionResult> Create()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new CreateBookingViewModel();
        var schedules = await _appBLL.Schedules
            .GettingAllOrderedSchedulesWithIncludesAsync(null, roleName);
        
        vm.Schedules = new SelectList(schedules,
            nameof(ScheduleDTO.Id), nameof(ScheduleDTO.ShiftDurationTime));
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id), nameof(CityDTO.CityName));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id), 
            nameof(VehicleTypeDTO.VehicleTypeName));
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id),
            $"{nameof(DriverDTO.AppUser)}.{nameof(
                DriverDTO.AppUser.LastAndFirstName)}");
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
            nameof(VehicleDTO.Id), 
            nameof(VehicleDTO.VehicleIdentifier));
        vm.Customers = new SelectList(await _appBLL.Customers.GettingAllOrderedCustomersAsync(),
            nameof(CustomerDTO.Id),
            $"{nameof(CustomerDTO.AppUser)}." +
            $"{nameof(CustomerDTO.AppUser.LastAndFirstName)}");

        return View(vm);
    }

    // POST: AdminArea/Bookings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area booking create POST method
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>New booking</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel vm)
    {
        var booking = new BookingDTO();
        if (ModelState.IsValid)
        {

            booking.Id = Guid.NewGuid();
            booking.CityId = vm.CityId;
            booking.CustomerId = vm.CustomerId;
            booking.DriverId = vm.DriverId;
            booking.ScheduleId = vm.ScheduleId!.Value;
            booking.VehicleId = vm.VehicleId;
            booking.AdditionalInfo = vm.AdditionalInfo;
            booking.DestinationAddress = vm.DestinationAddress;
            booking.PickupAddress = vm.PickupAddress;
            booking.VehicleTypeId = vm.VehicleTypeId;
            booking.HasAnAssistant = vm.HasAnAssistant;
            booking.NumberOfPassengers = vm.NumberOfPassengers;
            booking.StatusOfBooking = StatusOfBooking.Awaiting;
            booking.PickUpDateAndTime = DateTime.Parse(vm.PickUpDateAndTime).ToUniversalTime();
            booking.CreatedAt = DateTime.Now.ToUniversalTime();
            booking.CreatedBy = User.GettingUserEmail();
            booking.UpdatedAt = booking.CreatedAt;
            booking.UpdatedBy = booking.CreatedBy;
            
            _appBLL.Bookings.Add(booking);

            var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(vm.RideTimeId, null, null, true, true);
            if (rideTime != null)
            {
                rideTime.BookingId = booking.Id;
                rideTime.IsTaken = true;
                _appBLL.RideTimes.Update(rideTime);
            }

            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Schedules = new SelectList(await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(),
            nameof(ScheduleDTO.Id), nameof(ScheduleDTO.ShiftDurationTime),
            nameof(vm.ScheduleId));
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id),
            nameof(CityDTO.CityName), nameof(vm.CityId));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id), nameof(VehicleTypeDTO.VehicleTypeName)
            , nameof(vm.VehicleTypeId));
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id),
            $"{nameof(DriverDTO.AppUser)}." +
            $"{nameof(DriverDTO.AppUser.LastAndFirstName)}",
            nameof(vm.DriverId));
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(VehicleDTO.Id), 
            nameof(VehicleDTO.VehicleIdentifier),
            nameof(vm.VehicleId));
        vm.Customers = new SelectList(await _appBLL.Customers.GettingAllOrderedCustomersAsync(),
            nameof(CustomerDTO.Id),
            $"{nameof(CustomerDTO.AppUser)}" +
            $".{nameof(CustomerDTO.AppUser.LastAndFirstName)}",
            nameof(vm.CustomerId));

        return View(vm);
    }
    
    // GET: AdminArea/Bookings/Decline/5
    /// <summary>
    /// Admin area booking decline GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Decline(Guid? id)
    {
        var vm = new DeclineBookingViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, null, roleName, noIncludes:false);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        booking.Schedule!.StartDateAndTime = booking.Schedule!.StartDateAndTime;
        booking.Schedule!.EndDateAndTime = booking.Schedule!.EndDateAndTime;
        vm.ShiftDurationTime = booking.Schedule!.ShiftDurationTime;
        vm.City = booking.City!.CityName;
        vm.Driver = booking.Driver!.AppUser!.LastAndFirstName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt;
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = booking.UpdatedAt;

        return View(vm);
    }
    
    // POST: AdminArea/Bookings/Decline/5
    /// <summary>
    /// Admin area booking decline POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Declining the booking and redirect the user back to index page</returns>
    [HttpPost]
    [ActionName(nameof(Decline))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        
          var booking = await _appBLL.Bookings.BookingDeclineAsync(id, User.GettingUserId(), roleName, noIncludes:true, noTracking:true  );
            booking!.DeclineDateAndTime = DateTime.Now.ToUniversalTime();
            booking.IsDeclined = true;
            booking.DeclineDateAndTime = DateTime.Now.ToUniversalTime();
            booking.UpdatedBy = User.Identity!.Name;
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Bookings.Update(booking);
            await _appBLL.SaveChangesAsync();

            var drive = await _appBLL.Drives.GettingDriveByBookingIdAsync(booking.Id, noIncludes:true, noTracking:true);
            drive!.IsDriveDeclined = true;
            drive.StatusOfDrive = StatusOfDrive.Declined;
            drive.DriveDeclineDateAndTime = DateTime.Now.ToUniversalTime();
            drive.UpdatedBy = User.Identity!.Name;
            drive.UpdatedAt = DateTime.Now.ToUniversalTime();

            // Prepare Email Notification
            var mailRequest = new MailRequest();
            mailRequest.Subject = $"Booking Declined: {booking.PickUpDateAndTime.ToLocalTime():g} {booking.PickupAddress}";
            mailRequest.ToEmail = "programmeerija88@gmail.com";
            mailRequest.Body = $"Booking Declined: {booking.PickUpDateAndTime.ToLocalTime():g} {booking.PickupAddress}";

            using (MailMessage mm = new MailMessage("testitaxi88@gmail.com", mailRequest.ToEmail))
            {
                mm.Subject = mailRequest.Subject;
                mm.Body = mailRequest.Body;
                mm.IsBodyHtml = false;
                    
                using(SmtpClient smtp =  new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    NetworkCredential creds = new NetworkCredential("testitaxi88@gmail.com", "luaxcxmsxlvggmsz");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = creds;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }

            // Release the RideTime
            var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByBookingIdAsync
                (booking.Id, null, null, true, noIncludes:true);
            if (rideTime != null)
            {
                rideTime.BookingId = null;
                rideTime.ExpiryTime = null;
                rideTime.IsTaken = false;
                _appBLL.RideTimes.Update(rideTime);
            }

            await _appBLL.SaveChangesAsync();
            _appBLL.Drives.Update(drive);
            
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    // GET: AdminArea/Bookings/Delete/5
    /// <summary>
    /// Admin area booking delete GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();

        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, null, roleName );
        if (booking == null) return NotFound();
        vm.Id = booking.Id;
        booking.Schedule!.StartDateAndTime = booking.Schedule.StartDateAndTime.ToLocalTime();
        booking.Schedule.EndDateAndTime = booking.Schedule.EndDateAndTime.ToLocalTime();
        vm.ShiftDurationTime = booking.Schedule.ShiftDurationTime ;
        vm.City = booking.City!.CityName;
        vm.Driver = booking.Driver!.AppUser!.LastAndFirstName;
        vm.Customer = booking.Customer!.AppUser!.LastAndFirstName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.BookingDeclineDateAndTime = booking.DeclineDateAndTime.ToString("G");
        vm.IsDeclined = booking.IsDeclined;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt;
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = booking.UpdatedAt;

        return View(vm);
    }
    
    // POST: AdminArea/Bookings/Delete/5
    /// <summary>
    /// Admin area booking delete POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to an user to index page</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, null, roleName, noIncludes: true);
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByBookingIdAsync
            (id, null, null, true, true);
        
        if (rideTime != null)
        {
            rideTime.BookingId = null;
            rideTime.ExpiryTime = null;
            rideTime.IsTaken = false;
            _appBLL.RideTimes.Update(rideTime);
        }

        await _appBLL.SaveChangesAsync();

        if (booking != null)
        {
           await _appBLL.Bookings.RemoveAsync(booking.Id);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(Guid id)
    {
        return _appBLL.Bookings.Exists(id);
    }

    /// <summary>
    /// Search records by city name
    /// </summary>
    /// <param name="search">City name</param>
    /// <returns>An index view with search results</returns>
    [HttpPost]
    public async Task<IActionResult> SearchByCityAsync([FromForm] string search)
    {
        var roleName = User.GettingUserRoleName();
        var results = await _appBLL.Bookings.SearchByCityAsync(search, null, roleName );
        return View(nameof(Index), results);
    }
}