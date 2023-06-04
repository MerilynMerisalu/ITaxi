using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.CustomerArea.ViewModels;

namespace WebApp.Areas.CustomerArea.Controllers;

[Area(nameof(CustomerArea))]
[Authorize(Roles = "Admin, Customer")]
public class BookingsController : Controller
{
    private readonly IAppBLL _appBLL;

    public BookingsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }


    // GET: CustomerArea/Bookings
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(userId, roleName);
        
        
        return View(res);
        
    }
    
    
         /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    /// <param name="listType">the dropdownlist that has been changed</param>
    /// <param name="value">The currently selected item value</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody] AdminArea.Controllers.BookingsController.BookingSetDropDownListRequest parameters)
    {
        // Use the EditRideTimeViewModel because we want to send through the SelectLists and Ids that have now changed
        var vm = new CreateBookingViewModel();
        //IEnumerable<ScheduleDTO>? schedules = null;
        //Guid id = Guid.Parse(value);

        if (parameters.ListType == nameof(BookingDTO.PickUpDateAndTime))
        {
            // If the UI provides a RideTimeId, then we need to clear or release this time first
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
                        
                        vm.ScheduleId = bestTime.ScheduleId;
                        
                        vm.DriverId = bestTime.DriverId;
                        
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

        // This is not currently in use, this is theoretically what we would do if the user was
        // forced to select a specific RideTime, we have changed this and instead
        // the user is forced to enter a Pickup Date Time that matches an existing Ride Time
        if (parameters.ListType == "RideTimeId")
        {
            var rideTimeId = Guid.Parse(parameters.Value);
            var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(rideTimeId);

            vm.RideTimeId = rideTimeId;
            
            vm.ScheduleId = rideTime!.ScheduleId;
            
            vm.DriverId = rideTime.DriverId;
            vm.VehicleId = rideTime.Schedule!.VehicleId;
        }

        return Ok(vm);
    }

    // GET: CustomerArea/Bookings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, userId, roleName);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;

        vm.City = booking.City!.CityName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");

        return View(vm);
    }

    // GET: CustomerArea/Bookings/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateBookingViewModel();
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id), nameof(CityDTO.CityName));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id), nameof(VehicleTypeDTO.VehicleTypeName));

        return View(vm);
    }

    // POST: CustomerArea/Bookings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> Create(CreateBookingViewModel vm)
    {
        var booking = new App.BLL.DTO.AdminArea.BookingDTO();
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            
            booking.Id = Guid.NewGuid();
            booking.CityId = vm.CityId;
            booking.CustomerId = await _appBLL.Customers.GettingCustomerIdByAppUserIdAsync(userId);
            booking.AdditionalInfo = vm.AdditionalInfo;
            booking.DestinationAddress = vm.DestinationAddress;
            booking.PickupAddress = vm.PickupAddress;
            booking.VehicleTypeId = vm.VehicleTypeId;
            booking.HasAnAssistant = vm.HasAnAssistant;
            booking.NumberOfPassengers = vm.NumberOfPassengers;
            booking.StatusOfBooking = StatusOfBooking.Awaiting;
            booking.PickUpDateAndTime = DateTime.Parse(vm.PickUpDateAndTime).ToUniversalTime();

            var rideTimeLookup =
                await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(vm.RideTimeId, userId);
            booking.ScheduleId = rideTimeLookup!.ScheduleId;
            booking.DriverId = rideTimeLookup.Schedule!.DriverId;
            booking.VehicleId = rideTimeLookup.Schedule!.VehicleId;
            booking.CreatedAt = DateTime.Now.ToLocalTime();
            booking.CreatedBy = User.GettingUserEmail();
            booking.UpdatedAt = booking.CreatedAt;
            booking.UpdatedBy = booking.CreatedBy;
            _appBLL.Bookings.Add(booking);
            
            // Assign the Drive via the implicit related object creation
            var drive = new DriveDTO()
            {
                Id = new Guid(),
                DriverId = booking.DriverId,
                StatusOfDrive = StatusOfDrive.Awaiting
            };
            _appBLL.Drives.Add(drive);
            

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

        
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id),
            nameof(CityDTO.CityName), nameof(vm.CityId));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id), nameof(VehicleTypeDTO.VehicleTypeName)
            , nameof(vm.VehicleTypeId));
        
        return View(vm);
    }
    // GET: AdminArea/Bookings/Edit/5
    /*public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditBookingViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, userId, roleName);
        if (booking == null) return NotFound();


        vm.Cities = new SelectList(await _appBLL.Cities.GetAllCitiesWithoutCountyAsync()
            , nameof(City.Id), nameof(City.CityName));
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.CityId = booking.CityId;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleTypes = new SelectList(
            await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id)
            , nameof(VehicleType.VehicleTypeName));
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.VehicleTypeId = booking.VehicleTypeId;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime;
        return View(vm);
    }
    */
    

    // POST: AdminArea/Bookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
    //[HttpPost]
   // [ValidateAntiForgeryToken]
    /*public async Task<IActionResult> Edit(Guid id, EditBookingViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName, false);
        if (booking != null && id != booking.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var customerId = _appBLL.Customers
                .SingleOrDefaultAsync(c => c!.AppUserId.Equals(userId)).Result!.Id;
            try
            {
                if (booking != null)
                {
                    booking.Id = id;
                    booking.CityId = vm.CityId;
                    booking.CustomerId = customerId;
                    booking.DriverId = _appBLL.Drivers.FirstAsync().Result!.Id;
#warning needs fixing
                    booking.ScheduleId =  _appBLL.Schedules.FirstAsync().Result!.Id;
#warning needs fixing
                    booking.VehicleId = _appBLL.Vehicles.FirstAsync().Result!.Id;
                    booking.AdditionalInfo = vm.AdditionalInfo;
                    booking.DestinationAddress = vm.DestinationAddress;
                    booking.PickupAddress = vm.PickupAddress;
                    booking.VehicleTypeId = vm.VehicleTypeId;
                    booking.HasAnAssistant = vm.HasAnAssistant;
                    booking.NumberOfPassengers = vm.NumberOfPassengers;
                    booking.StatusOfBooking = StatusOfBooking.Awaiting;
                    booking.PickUpDateAndTime = vm.PickUpDateAndTime;


                    _appBLL.Bookings.Update(booking);
                }

                var drive = await _appBLL.Drives
                    .SingleOrDefaultAsync(d => d!.Booking!.Id.Equals(booking!.Id), false);
                if (drive != null)
                {
                    if (booking != null)
                    {
                        drive.DriverId = booking.DriverId;
                        drive.Booking = booking;
                    }

                    _appBLL.Drives.Update(drive);
                    await _appBLL.SaveChangesAsync();
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
    }*/

    // GET: CustomerArea/Bookings/Decline/5
    public async Task<IActionResult> Decline(Guid? id)
    {
        var vm = new DeclineBookingViewModel();
        if (id == null) return NotFound();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, userId, roleName);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        vm.City = booking.City!.CityName;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");


        return View(vm);
    }

    // POST: AdminArea/Bookings/Decline/5
    [HttpPost]
    [ActionName(nameof(Decline))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName, false);
        if (booking != null)
        {
            await _appBLL.Bookings.BookingDeclineAsync(booking.Id, userId, roleName);
        }
        return RedirectToAction(nameof(Index));
    }


    // GET: CustomerArea/Bookings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id.Value, userId, roleName, false);
        if (booking == null) return NotFound();
        vm.Id = booking.Id;
        vm.City = booking.City!.CityName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        return View(vm);
    }


    // POST: CustomerArea/Bookings/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName );
        var drive = await _appBLL.Drives.SingleOrDefaultAsync(d => d != null && d.Booking!.Id.Equals(id), false);
        var comment =
            await _appBLL.Comments.SingleOrDefaultAsync(c => drive != null && c != null && c.DriveId.Equals(drive.Id),
                false);
        if (comment != null) await _appBLL.Comments.RemoveAsync(comment.Id);
        if (drive != null) await _appBLL.Drives.RemoveAsync(drive.Id);
        if (booking != null) await _appBLL.Bookings.RemoveAsync(booking.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(Guid id)
    {
        return _appBLL.Bookings.Exists(id);
    }

    /// <summary>
    ///     Search records by city name
    /// </summary>
    /// <param name="search">City name</param>
    /// <returns>An index view with search results</returns>
    [HttpPost]
    public async Task<IActionResult> SearchByCityAsync([FromForm] string search)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var results = await _appBLL.Bookings.SearchByCityAsync(search, userId, roleName);
        return View(nameof(Index), results);
    }
    
}