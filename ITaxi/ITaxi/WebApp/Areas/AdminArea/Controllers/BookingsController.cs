#nullable enable
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using WebApp.Helpers;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class BookingsController : Controller
{
    private readonly IMailService _mailService;
    private readonly IAppUnitOfWork _uow;

    public BookingsController(IAppUnitOfWork uow, IMailService mailService)
    {
        _uow = uow;
        _mailService = mailService;
    }


    // GET: AdminArea/Bookings
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();
#warning Should this be a repo method
        var res = await _uow.Bookings.GettingAllOrderedBookingsAsync(null, roleName);
        foreach (var booking in res)
            if (booking != null)
            {
                booking.Schedule!.StartDateAndTime = booking.Schedule!.StartDateAndTime.ToLocalTime();
                booking.Schedule!.EndDateAndTime = booking.Schedule!.EndDateAndTime.ToLocalTime();
                booking.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime();
                booking.CreatedAt = booking.CreatedAt.ToLocalTime();
                booking.UpdatedAt = booking.UpdatedAt.ToLocalTime();
                booking.DeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime();
            }

        return View(res);
    }

    // GET: AdminArea/Bookings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id.Value, null, roleName);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        
        booking.Schedule!.StartDateAndTime = booking.Schedule!.StartDateAndTime.ToLocalTime();
        booking.Schedule!.EndDateAndTime = booking.Schedule!.EndDateAndTime.ToLocalTime();
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
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.BookingDeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime().ToString("G");
        vm.IsDeclined = booking.IsDeclined;
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = booking.UpdatedBy!;
        vm.UpdatedAt = booking.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }
    
    public class BookingSetDropDownListRequest
    {
        public string ListType { get; set; }
        public string Value { get; set; }
        
        public Guid VehicleTypeId { get; set; }
        public Guid CityId { get; set; }

        public int NumberOfPassengers { get; set; }
        
        public DateTime PickupDateAndTime { get; set; } 
    }
    /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    /// <param name="listType">the dropdownlist that has been changed</param>
    /// <param name="value">The currently selected item value</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody]BookingSetDropDownListRequest parameters)
    {
        // Use the EditRideTimeViewModel because we want to send through the SelectLists and Ids that have now changed
        var vm = new CreateBookingViewModel();
        IEnumerable<Schedule> schedules = null;
        //Guid id = Guid.Parse(value);

        if (parameters.ListType == nameof(Booking.PickUpDateAndTime))
        {
            // Value in this case IS the pickupDateAndTime
            parameters.PickupDateAndTime = DateTime.Parse(parameters.Value);
            if (parameters.PickupDateAndTime == DateTime.MinValue)
            {
                // Do nothing, this will show the message to re-select the time entry
            }
            else
            {
                parameters.PickupDateAndTime = parameters.PickupDateAndTime.ToUniversalTime();

                // We want to find the nearest available ride date and time
                // First we get a list of the available ride times
                var bestTimes = await _uow.RideTimes.GettingBestAvailableRideTimeAsync(parameters.PickupDateAndTime,
                    parameters.CityId, parameters.NumberOfPassengers, parameters.VehicleTypeId, true);
                if (bestTimes.Any())
                {
                    if (bestTimes.Count == 1)
                    {
                        var bestTime = bestTimes.First();

                        vm.Schedules = new SelectList(new[] {bestTime.Schedule}, nameof(Schedule.Id),
                            nameof(Schedule.ShiftDurationTime));
                        vm.ScheduleId = bestTime.ScheduleId;
                        vm.Drivers = new SelectList(new[] {bestTime.Driver}, nameof(Driver.Id),
                            "AppUser.LastAndFirstName");
                        vm.DriverId = bestTime.DriverId;
                        vm.Vehicles = new SelectList(new[] {bestTime.Schedule!.Vehicle}, nameof(Vehicle.Id),
                            nameof(Vehicle.VehicleIdentifier));
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
                                Value = x.RideDateTime.ToLocalTime().ToString("yyyy-MM-ddThh:mm"),
                                Text = x.RideDateTime.ToLocalTime().ToString("g")
                            })
                            .ToList();
                        if (timesForDisplay.Any())
                            vm.RideTimes = new SelectList(timesForDisplay, "Value", "Text");
                    }
                }
            }
        }

        if (parameters.ListType == "RideTimeId")
        {
            var rideTimeId = Guid.Parse(parameters.Value);
            var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(rideTimeId);
            
            vm.RideTimeId = rideTimeId;
            vm.Schedules = new SelectList(new[] {rideTime.Schedule}, nameof(Schedule.Id),
                nameof(Schedule.ShiftDurationTime));
            vm.ScheduleId = rideTime.ScheduleId;
            vm.Drivers = new SelectList(new[] {rideTime.Driver}, nameof(Driver.Id),
                "AppUser.LastAndFirstName");
            vm.DriverId = rideTime.DriverId;
            vm.Vehicles = new SelectList(new[] {rideTime.Schedule!.Vehicle }, nameof(Vehicle.Id),
                nameof(Vehicle.VehicleIdentifier));
            vm.VehicleId = rideTime.Schedule!.VehicleId;
        }
        
        return Ok(vm);
    }

    // GET: AdminArea/Bookings/Create
    public async Task<IActionResult> Create()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new CreateBookingViewModel();
        var schedules = await _uow.Schedules
            .GettingAllOrderedSchedulesWithIncludesAsync(null, roleName);
        foreach (var schedule in schedules)
        {
            schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
            schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();

        }
        vm.Schedules = new SelectList(schedules,
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName));
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id),
            $"{nameof(Driver.AppUser)}.{nameof(Driver.AppUser.LastAndFirstName)}");
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier));
        vm.Customers = new SelectList(await _uow.Customers.GettingAllOrderedCustomersAsync(),
            nameof(Customer.Id),
#warning "Magic string" code smell, fix it
            $"{nameof(Customer.AppUser)}.{nameof(Customer.AppUser.LastAndFirstName)}");
        

        return View(vm);
    }

    // POST: AdminArea/Bookings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel vm)
    {
        var booking = new Booking();
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
#warning Booking PickUpDateAndTime needs a custom validation
            booking.PickUpDateAndTime = DateTime.Parse(vm.PickUpDateAndTime).ToUniversalTime();
            _uow.Bookings.Add(booking);

            var drive = new Drive
            {
                Id = new Guid(),
                Booking = booking,
                DriverId = booking.DriverId,
                StatusOfDrive = StatusOfDrive.Awaiting
            };

            _uow.Drives.Add(drive);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(),
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime),
            nameof(vm.ScheduleId));
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName), nameof(vm.CityId));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName)
            , nameof(vm.VehicleTypeId));
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id),
            $"{nameof(Driver.AppUser)}.{nameof(Driver.AppUser.LastAndFirstName)}",
            nameof(vm.DriverId));
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier),
            nameof(vm.VehicleId));
        vm.Customers = new SelectList(await _uow.Customers.GettingAllOrderedCustomersAsync(),
            nameof(Customer.Id),
#warning "Magic string" code smell, fix it
            $"{nameof(Customer.AppUser)}.{nameof(Customer.AppUser.LastAndFirstName)}",
            nameof(vm.CustomerId));

        return View(vm);
    }

    // GET: AdminArea/Bookings/Edit/5
    /*public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditBookingViewModel();
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id.Value, null, roleName);
        if (booking == null) return NotFound();
        
        // For edit, only load 1 item into each drop down list
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(booking.ScheduleId);
        schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
        schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
        vm.Schedules = new SelectList(new [] {schedule},
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        vm.ScheduleId = booking.ScheduleId;
        #warning Change this to Get just 1 driver by Id from the uow
        //var driver = _
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id),
            $"{nameof(Driver.AppUser)}.{nameof(Driver.AppUser.LastAndFirstName)}");
        vm.DriverId = booking.DriverId;
        vm.Customers = new SelectList(await _uow.Customers.GettingAllOrderedCustomersAsync(),
            nameof(Customer.Id),
            $"{nameof(Customer.AppUser)}.{nameof(Customer.AppUser.LastAndFirstName)}");
        vm.CustomerId = booking.CustomerId;
        vm.Cities = new SelectList(await _uow.Cities.GetAllCitiesWithoutCountyAsync()
            , nameof(City.Id), nameof(City.CityName));
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(null, roleName),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier));
        vm.VehicleId = booking.VehicleId;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.CityId = booking.CityId;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleTypes = new SelectList(
            await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id)
            , nameof(VehicleType.VehicleTypeName));
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.VehicleTypeId = booking.VehicleTypeId;
        vm.PickUpDateAndTime = Convert.ToDateTime(booking.PickUpDateAndTime.ToLocalTime().ToString("g"));
        return View(vm);
    }
    */

    // POST: AdminArea/Bookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditBookingViewModel vm)
    {
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id, null, roleName);
        if (booking != null && id != booking.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (booking != null)
                {
                    booking.Id = id;
                    booking.ScheduleId = vm.ScheduleId;
                    booking.CityId = vm.CityId;
                    booking.CustomerId = vm.CustomerId;
                    booking.DriverId = vm.DriverId;
                    booking.ScheduleId = vm.ScheduleId;
                    booking.VehicleId = vm.VehicleId;
                    booking.AdditionalInfo = vm.AdditionalInfo;
                    booking.DestinationAddress = vm.DestinationAddress;
                    booking.PickupAddress = vm.PickupAddress;
                    booking.VehicleTypeId = vm.VehicleTypeId;
                    booking.HasAnAssistant = vm.HasAnAssistant;
                    booking.NumberOfPassengers = vm.NumberOfPassengers;
                    booking.StatusOfBooking = StatusOfBooking.Awaiting;
                    booking.PickUpDateAndTime = vm.PickUpDateAndTime.ToUniversalTime();
                    booking.UpdatedBy = User.Identity!.Name;
                    booking.UpdatedAt = DateTime.Now.ToUniversalTime();

                    _uow.Bookings.Update(booking);
                }

                var drive = await _uow.Drives
                    .SingleOrDefaultAsync(d => d!.Booking!.Id.Equals(booking!.Id), false);
                if (drive != null)
                {
                    if (booking != null)
                    {
                        drive.DriverId = booking.DriverId;
                        drive.Booking = booking;
                    }

                    _uow.Drives.Update(drive);
                    await _uow.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (booking != null && !BookingExists(booking.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    } */

    // GET: AdminArea/Bookings/Decline/5
    public async Task<IActionResult> Decline(Guid? id)
    {
        var vm = new DeclineBookingViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id.Value, null, roleName);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        booking.Schedule!.StartDateAndTime = booking.Schedule!.StartDateAndTime.ToLocalTime();
        booking.Schedule!.EndDateAndTime = booking.Schedule!.EndDateAndTime.ToLocalTime();
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
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = booking.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }
    
    // POST: AdminArea/Bookings/Decline/5
    [HttpPost]
    [ActionName(nameof(Decline))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id, null, roleName);
        if (booking != null)
        {
            var drive = await _uow.Drives.SingleOrDefaultAsync(d => d!.Booking!.Id.Equals(id), false);
            await _uow.Bookings.BookingDeclineAsync(booking.Id, null, roleName );
            booking.DeclineDateAndTime = DateTime.Now.ToUniversalTime();
            booking.IsDeclined = true;
            booking.DeclineDateAndTime = DateTime.Now.ToUniversalTime();
            booking.UpdatedBy = User.Identity!.Name;
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
            drive!.Booking = booking;
            _uow.Bookings.Update(booking);
            drive.UpdatedBy = User.Identity!.Name;
            drive.UpdatedAt = DateTime.Now.ToUniversalTime();
#warning refactor into a common service for bookings
#warning Add an EmailAddress Field to Driver with the "~Real"~address
#warning Add a language field to Driver
            // Prepare Email Notification
            var mailRequest = new MailRequest();
#warning Add Language Support for email templates
            mailRequest.Subject = $"Booking Declined: {booking.PickUpDateAndTime:g} {booking.PickupAddress}";
            mailRequest.ToEmail = "programmeerija88@gmail.com";
#warning Include a link to quick login to the portal to see this booking in the email content
            mailRequest.Body = $"Booking Declined: {booking.PickUpDateAndTime:g} {booking.PickupAddress}";

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
            

            // Send Email Notification
            //_mailService.
            var response = await _mailService.SendEmailAsync(mailRequest);
            Trace.WriteLine(response);

            await _uow.SaveChangesAsync();
            _uow.Drives.Update(drive);
        }

        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    // GET: AdminArea/Bookings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();

        var booking = await _uow.Bookings.GettingBookingAsync(id.Value, null, roleName );
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
        vm.BookingDeclineDateAndTime = booking.DeclineDateAndTime.ToLocalTime().ToString("G");
        vm.IsDeclined = booking.IsDeclined;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = booking.CreatedBy!;
        vm.CreatedAt = booking.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = booking.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }

   

    // POST: AdminArea/Bookings/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var booking = await _uow.Bookings.GettingBookingAsync(id, null,roleName );
        var drive = await _uow.Drives.SingleOrDefaultAsync(d => d != null && d.Booking!.Id.Equals(id), false);
        var comment =
            await _uow.Comments.SingleOrDefaultAsync(c => drive != null && c != null && c.DriveId.Equals(drive.Id),
                false);
        if (comment != null) _uow.Comments.Remove(comment);
        if (drive != null) _uow.Drives.Remove(drive);
        if (booking != null) _uow.Bookings.Remove(booking);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(Guid id)
    {
        return _uow.Bookings.Exists(id);
    }

    /// <summary>
    ///     Search records by city name
    /// </summary>
    /// <param name="search">City name</param>
    /// <returns>An index view with search results</returns>
    [HttpPost]
    public async Task<IActionResult> SearchByCityAsync([FromForm] string search)
    {
        var roleName = User.GettingUserRoleName();
        var results = await _uow.Bookings.SearchByCityAsync(search, null, roleName );
        return View(nameof(Index), results);
    }
}