using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using WebApp.Areas.CustomerArea.ViewModels;

namespace WebApp.Areas.CustomerArea.Controllers;

/// <summary>
/// Customer area booking controller
/// </summary>
[Area(nameof(CustomerArea))]
[Authorize(Roles = "Admin, Customer")]
public class BookingsController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Customer area booking controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public BookingsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }
    
    // GET: CustomerArea/Bookings
    /// <summary>
    /// Customer area booking index
    /// </summary>
    /// <returns>View</returns>
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
    /// <param name="parameters">Parameters</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody] AdminArea.Controllers.BookingsController.BookingSetDropDownListRequest parameters)
    {
        // Using the EditRideTimeViewModel because I want to send through the SelectLists and Ids that have now changed
        var vm = new CreateBookingViewModel();
        
        // ListType:The dropdownlist that has been changed
        // Value: The currently selected item value
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
            parameters.PickupDateAndTime = DateTime.Parse(parameters!.Value!);
            if (parameters.PickupDateAndTime == DateTime.MinValue)
            {
                // Doing nothing, this will show the message to re-select the time entry
            }
            else
            {
                parameters.PickupDateAndTime = parameters.PickupDateAndTime.ToUniversalTime();

                // I want to find the nearest available ride date and time
                // First i get a list of the available ride times
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

        // This is not currently in use, this is theoretically what I would do if the user was
        // forced to select a specific RideTime, I have changed this and instead
        // the user is forced to enter a Pickup Date Time that matches an existing Ride Time
        if (parameters.ListType == "RideTimeId")
        {
            var rideTimeId = Guid.Parse(parameters.Value!);
            var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(rideTimeId);

            vm.RideTimeId = rideTimeId;
            vm.ScheduleId = rideTime!.ScheduleId;
            vm.DriverId = rideTime.DriverId;
            vm.VehicleId = rideTime.Schedule!.VehicleId;
        }
        return Ok(vm);
    }

    // GET: CustomerArea/Bookings/Details/5
    /// <summary>
    /// Customer area booking GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        
        vm.VehicleData = booking.VehiclesData;
        vm.DriverData = booking.DriversData;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.NeedAssistanceEnteringTheBuilding = booking.NeedAssistanceEnteringTheBuilding;
        if (booking.NeedAssistanceEnteringTheBuilding == true)
            vm.DestinationFloorNumber = booking.DestinationFloorNumber;
        vm.PickupAddress = booking.PickupAddress;
        vm.NeedAssistanceLeavingTheBuilding = booking.NeedAssistanceLeavingTheBuilding;
        if (booking.NeedAssistanceLeavingTheBuilding)
        {
            vm.PickupFloorNumber = booking.PickupFloorNumber;
        }
        
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");

        return View(vm);
    }

    // GET: CustomerArea/Bookings/Create
    /// <summary>
    /// Customer area booking GET method create
    /// </summary>
    /// <returns>View</returns>
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
    /// <summary>
    /// Customer area booking POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel vm)
    {
        var booking = new BookingDTO();
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            
            booking.Id = Guid.NewGuid();
            booking.CityId = vm.CityId;
            booking.CustomerId = await _appBLL.Customers.GettingCustomerIdByAppUserIdAsync(userId);
            booking.AdditionalInfo = vm.AdditionalInfo;
            booking.DestinationAddress = vm.DestinationAddress;
            booking.NeedAssistanceEnteringTheBuilding = vm.NeedAssistanceEnteringTheBuilding;
            if (vm.NeedAssistanceEnteringTheBuilding)
                booking.DestinationFloorNumber = vm.DestinationFloorNumber;
            booking.PickupAddress = vm.PickupAddress;
            booking.NeedAssistanceLeavingTheBuilding = vm.NeedAssistanceLeavingTheBuilding;
            if(vm.NeedAssistanceLeavingTheBuilding)
                booking.PickupFloorNumber = vm.PickupFloorNumber;
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
            await _appBLL.SaveChangesAsync();
            // Assign the Drive via the implicit related object creation
            var drive = new DriveDTO()
            {
                Id = new Guid(),
                DriverId = booking.DriverId,
                StatusOfDrive = StatusOfDrive.Awaiting
            };
            _appBLL.Drives.Add(drive);
            await _appBLL.SaveChangesAsync();
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

    // GET: CustomerArea/Bookings/Decline/5
    /// <summary>
    /// Customer area booking GET method decline
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        vm.NeedAssistanceEnteringTheBuilding = booking.NeedAssistanceEnteringTheBuilding;
        if (booking.NeedAssistanceLeavingTheBuilding)
        {
            vm.DestinationFloorNumber = booking.DestinationFloorNumber;
            vm.HasAnElevatorInTheDestinationBuilding = booking.HasAnElevatorInTheDestinationBuilding;
        }
            
        vm.PickupAddress = booking.PickupAddress;
        vm.NeedAssistanceLeavingTheBuilding = booking.NeedAssistanceLeavingTheBuilding;
        if (booking.NeedAssistanceLeavingTheBuilding)
        {
            vm.PickupFloorNumber = booking.PickupFloorNumber;
            vm.HasAnElevatorInThePickupBuilding = booking.HasAnElevatorInThePickupBuilding;
        }
        vm.VehicleData = booking.VehiclesData;
        vm.DriverData = booking.DriversData;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        
        return View(vm);
    }

    // POST: AdminArea/Bookings/Decline/5
    /// <summary>
    /// Customer area booking POST method decline
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
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
    /// <summary>
    /// Customer area booking GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        vm.VehicleData = booking!.VehiclesData;
        vm.VehicleType = booking!.VehicleType!.VehicleTypeName;
        vm.DriverData = booking!.DriversData;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.NeedAssistanceEnteringTheBuilding = booking.NeedAssistanceEnteringTheBuilding;
        if (booking.NeedAssistanceEnteringTheBuilding)
        {
            vm.DestinationFloorNumber = booking.DestinationFloorNumber;
            vm.HasAnElevatorInTheDestinationBuilding = booking.HasAnElevatorInTheDestinationBuilding;
        }
        vm.PickupAddress = booking.PickupAddress;
        vm.NeedAssistanceLeavingTheBuilding = booking.NeedAssistanceLeavingTheBuilding;
        if (booking.NeedAssistanceLeavingTheBuilding)
        {
            vm.PickupFloorNumber = booking.PickupFloorNumber;
            vm.HasAnElevatorInThePickupBuilding = booking.HasAnElevatorInThePickupBuilding;
        }
            
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        return View(vm);
    }
    
    // POST: CustomerArea/Bookings/Delete/5
    /// <summary>
    /// Customer area booking POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var booking = await _appBLL.Bookings.GettingBookingAsync(id, userId, roleName, noIncludes:true );
        var drive = await _appBLL.Drives.GettingDriveByBookingIdAsync(booking!.Id, noIncludes:true);
        var comment =
            await _appBLL.Comments.GettingCommentByDriveIdAsync(drive!.Id, noIncludes:true);
        if (comment != null) await _appBLL.Comments.RemoveAsync(comment.Id);
        await _appBLL.SaveChangesAsync();
        if (drive != null) await _appBLL.Drives.RemoveAsync(drive.Id);
        await _appBLL.SaveChangesAsync();
        if (booking != null) await _appBLL.Bookings.RemoveAsync(booking.Id);
        await _appBLL.SaveChangesAsync();
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var results = await _appBLL.Bookings.SearchByCityAsync(search, userId, roleName);
        return View(nameof(Index), results);
    }

    /// <summary>
    /// Adding a pdf view for customer's bookings
    /// </summary>
    /// <returns>pdf view for customer's bookings</returns>
    public async Task<IActionResult> PrintAsync()
    {
        var userId = User.GettingUserId();
        var bookings = await _appBLL.Bookings.GettingAllOrderedBookingsAsync(userId);
        return new ViewAsPdf("PrintBookings", bookings);
    }
}