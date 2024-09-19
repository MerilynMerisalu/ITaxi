#nullable enable

using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area drives controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class DrivesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area drives controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public DrivesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Drives
    /// <summary>
    /// Admin area drives controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Drives.GettingAllOrderedDrivesWithIncludesAsync(null, roleName);

        return View(res);
    }

    
/// <summary>
/// Search drives by inserted date
/// </summary>
/// <param name="search">Date</param>
/// <returns>An index view with search results</returns>
[HttpPost]
public async Task<IActionResult> SearchByDateAsync([FromForm] DateTime search)
{
    var roleName = User.GettingUserRoleName();
    var drives = await _appBLL.Drives.SearchByDateAsync(search, null, roleName);
    return View(nameof(Index), drives);
}

/// <summary>
/// Generates a pdf view of drives
/// </summary>
/// <returns>A pdf of drives</returns>
public async Task<IActionResult> Print()
{
    var roleName = User.GettingUserRoleName();
    
    var drives = await _appBLL.Drives.PrintAsync( null, roleName );
        
    return new ViewAsPdf("PrintDrives", drives) 
    {
        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
    };
}

// GET: AdminArea/Drives/Accept/5
/// <summary>
/// Admin area drives controller GET method accept
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> Accept(Guid? id)
{
    if (id == null) return NotFound();

    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking!.Schedule!.StartDateAndTime;
    drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.NeedAssistanceLeavingTheBuilding = drive.Booking!.NeedAssistanceLeavingTheBuilding;
    if (drive.Booking!.NeedAssistanceLeavingTheBuilding)
            vm.PickupFloorNumber = drive.Booking!.PickupFloorNumber;
    vm.NeedAssistanceEnteringTheBuilding = drive.Booking!.NeedAssistanceEnteringTheBuilding;
        if (drive.Booking!.NeedAssistanceEnteringTheBuilding)
            vm.DestinationFloorNumber = drive.Booking!.DestinationFloorNumber; 
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("G");
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    if (vm.IsDriveAccepted )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("G");
    }
    if (vm.IsDriveDeclined )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("G");
        vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("G");
    }

    if (vm.IsDriveStarted)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("G");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("G");
    }

    if (vm.IsDriveFinished)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("G");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("G");
        vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("G");
    }

    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    vm.CreatedBy = drive.CreatedBy!;
    vm.CreatedAt = drive.CreatedAt;
    vm.UpdatedBy = drive.UpdatedBy!;
    vm.UpdatedAt = drive.UpdatedAt;
    
    return View(vm);
}

// POST: AdminArea/Bookings/Accept/5
/// <summary>
/// Admin area drives controller POST method accept
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(Accept))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AcceptConfirmed(Guid id)
{
    var roleName = User.GettingUserRoleName();
    var drive = await _appBLL.Drives.AcceptingDriveAsync(id, null, roleName, noIncludes:true);
    if (drive == null) return NotFound();

    drive.DriveAcceptedDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Accepted;
    drive.IsDriveAccepted = true;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();
    
    var booking = await _appBLL.Bookings.GettingBookingByDriveIdAsync(id, noTracking:true);
    
    if (booking != null)
    {
        booking.StatusOfBooking = StatusOfBooking.Accepted;
        booking.ConfirmedBy = User.GettingUserName();
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Bookings.Update(booking);
        await _appBLL.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

// GET: AdminArea/Drives/Decline/5
/// <summary>
/// Admin area drives controller GET method decline
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> Decline(Guid? id)
{
    if (id == null) return NotFound();

    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.NeedAssistanceEnteringTheBuilding = drive.Booking.NeedAssistanceEnteringTheBuilding;
    if(vm.NeedAssistanceEnteringTheBuilding)
        vm.DestinationFloorNumber = drive.Booking.DestinationFloorNumber;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.NeedAssistanceLeavingTheBuilding = drive.Booking.NeedAssistanceLeavingTheBuilding;
    if(vm.NeedAssistanceLeavingTheBuilding)
        vm.PickupFloorNumber = drive.Booking.PickupFloorNumber;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("G");
    vm.CreatedBy = drive.CreatedBy!;
    vm.CreatedAt = drive.CreatedAt;
    vm.UpdatedBy = drive.UpdatedBy!;
    vm.UpdatedAt = drive.UpdatedAt;
    
    return View(vm);
}

// POST: AdminArea/Drives/Decline/5
/// <summary>
/// Admin area drives controller POST method decline
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(Decline))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeclineConfirmed(Guid id)
{
    var roleName = User.GettingUserRoleName();
    var drive = await _appBLL.Drives.DecliningDriveAsync(id, null, roleName, noTracking:true, noIncludes:true);
    if (drive == null) return NotFound();

    drive.DriveDeclineDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Declined;
    drive.IsDriveDeclined = true;
    drive.UpdatedBy = User.Identity!.Name;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();

    var booking = await _appBLL.Bookings.GettingBookingByDriveIdAsync(id);
    if (booking != null)
    {
        booking.StatusOfBooking = StatusOfBooking.Declined;
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Bookings.Update(booking);
        
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByBookingIdAsync(booking.Id, null, null, true, noIncludes:true);
        if (rideTime != null)
        {
            rideTime.BookingId = null;
            rideTime.ExpiryTime = null;
            rideTime.IsTaken = false;
            _appBLL.RideTimes.Update(rideTime);
        }

        await _appBLL.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

// GET: AdminArea/Drives/StartDrive/5
/// <summary>
/// Admin area drives controller GET method start drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> StartDrive(Guid? id)
{
    if (id == null) return NotFound();
    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.NeedAssistanceEnteringTheBuilding = drive.Booking.NeedAssistanceEnteringTheBuilding;
    if (vm.NeedAssistanceEnteringTheBuilding) 
        vm.DestinationFloorNumber = drive.Booking.DestinationFloorNumber;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.NeedAssistanceLeavingTheBuilding = drive.Booking.NeedAssistanceLeavingTheBuilding;
    if (vm.NeedAssistanceLeavingTheBuilding)
        vm.PickupFloorNumber = drive.Booking.PickupFloorNumber;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    vm.CreatedBy = drive.CreatedBy!;
    vm.CreatedAt = drive.CreatedAt;
    vm.UpdatedBy = drive.UpdatedBy!;
    vm.UpdatedAt = drive.UpdatedAt;

    return View(vm);
}

// POST: AdminArea/Drives/Start/5
/// <summary>
/// Admin area drives controller POST method start
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(StartDrive))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> StartConfirmed(Guid id)
{
    var roleName = User.GettingUserRoleName();
    var drive = await _appBLL.Drives.StartingDriveAsync(id, null, roleName);
    if (drive == null) return NotFound();

    drive.DriveStartDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Started;
    drive.IsDriveStarted = true;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

// GET: AdminArea/Drives/EndDrive/5
/// <summary>
/// Admin area drives controller GET method end drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> EndDrive(Guid? id)
{
    if (id == null) return NotFound();

    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.NeedAssistanceEnteringTheBuilding = drive.Booking.NeedAssistanceEnteringTheBuilding;
    if (vm.NeedAssistanceEnteringTheBuilding) 
        vm.DestinationFloorNumber = drive.Booking.DestinationFloorNumber;
    vm.PickupAddress = drive.Booking.PickupAddress;
    if (vm.NeedAssistanceLeavingTheBuilding)
        vm.PickupFloorNumber = drive.Booking.PickupFloorNumber;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    vm.CreatedBy = drive.CreatedBy!;
    vm.CreatedAt = drive.CreatedAt;
    vm.UpdatedBy = drive.UpdatedBy!;
    vm.UpdatedAt = drive.UpdatedAt;

    return View(vm);
}


// POST: AdminArea/Drives/EndDrive/5
/// <summary>
/// Admin area drives controller POST method end drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(EndDrive))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EndDriveConfirmed(Guid id)
{
    var roleName = User.GettingUserRoleName();
    var drive = await _appBLL.Drives.EndingDriveAsync(id, null, roleName);
    if (drive == null) return NotFound();

    drive.IsDriveFinished = true;
    drive.StatusOfDrive = StatusOfDrive.Finished;
    drive.DriveEndDateAndTime = DateTime.Now.ToUniversalTime();
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();
    
    return RedirectToAction(nameof(Index));
}
}