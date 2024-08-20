#nullable enable
using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using WebApp.Areas.DriverArea.ViewModels;

namespace WebApp.Areas.DriverArea.Controllers;

/// <summary>
/// Driver area drives controller
/// </summary>
[Area(nameof(DriverArea))]
[Authorize(Roles = "Admin, Driver")]
public class DrivesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Driver area drives controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public DrivesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: DriverArea/Drives
    /// <summary>
    /// Driver area drives index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Drives.GettingAllOrderedDrivesWithIncludesAsync(userId, roleName);
        
        return View(res);
    }

    // GET: DriverArea/Drives/Details/5
    /// <summary>
    /// Driver area drives GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new DetailsDriveViewModel();
        if (id == null) return NotFound();

        var drive = await _appBLL.Drives
            .GettingFirstDriveAsync(id.Value, userId, roleName);
        if (drive == null) return NotFound();
        
        vm.Id = drive.Id;
        vm.City = drive.Booking!.City!.CityName;
        drive.Booking.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime;
        drive.Booking.Schedule!.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");

        if (drive.Comment?.CommentText != null) vm.CommentText = drive.Comment.CommentText;
        if (drive.Comment?.StarRating > 0)
        {
            vm.StarRating = drive.Comment.StarRating;
        }
        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        vm.IsDriveAccepted = drive.IsDriveAccepted;
        vm.IsDriveDeclined = drive.IsDriveDeclined;
        vm.IsDriveStarted = drive.IsDriveStarted;
        vm.IsDriveFinished = drive.IsDriveFinished;
        if (vm.IsDriveAccepted)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        }

        if (vm.IsDriveDeclined)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
            vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("g");
        }

        if (vm.IsDriveStarted)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
            vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
        }

        if (vm.IsDriveFinished)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
            vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
            vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("g");
        }
        
        return View(vm);
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
    var userId = User.GettingUserId();
    var drives = await _appBLL.Drives.SearchByDateAsync(search, userId, roleName);
    return View(nameof(Index), drives);
}

/// <summary>
/// Generates a pdf view of drives
/// </summary>
/// <returns>A pdf of drives</returns>
public async Task<IActionResult> Print()
{
    var roleName = User.GettingUserRoleName();
    var userId = User.GettingUserId();
    var drives = await _appBLL.Drives.PrintAsync( userId, roleName );
    
    return new ViewAsPdf("PrintDrives", drives);
}

// GET: DriverArea/Drives/Accept/5
/// <summary>
/// Driver area drives GET method accept
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> Accept(Guid? id)
{
    if (id == null) return NotFound();

    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, userId, roleName);
    
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime; 
    drive.Booking.Schedule!.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
  
    if (vm.IsDriveAccepted )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
    }
    
    if (vm.IsDriveDeclined )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("g");
    }

    if (vm.IsDriveStarted)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
    }

    if (vm.IsDriveFinished)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
        vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("g");
    }
    return View(vm);
}

// POST: DriverArea/Bookings/Accept/5
/// <summary>
/// Driver area drives controller POST method accept
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(Accept))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AcceptConfirmed(Guid id)
{
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    
    var drive = await _appBLL.Drives.AcceptingDriveAsync(id, userId, roleName);
    if (drive == null) return NotFound();

    drive.DriveAcceptedDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Accepted;
    drive.IsDriveAccepted = true;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();

    var booking = await _appBLL.Bookings.GettingBookingByDriveIdAsync(id, userId, roleName, noTracking:true, noIncludes:true);
    if (booking != null)
    {
        booking.StatusOfBooking = StatusOfBooking.Accepted;
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Bookings.Update(booking);
        await _appBLL.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

// GET: DriverArea/Drives/Decline/5
/// <summary>
/// Driver area drives GET method decline
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> Decline(Guid? id)
{
    if (id == null) return NotFound();
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();

    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, userId, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime; 
    drive.Booking.Schedule!.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    
    if (vm.IsDriveAccepted )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
    }
    
    if (vm.IsDriveDeclined )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("g");
    }

    if (vm.IsDriveStarted)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
    }

    if (vm.IsDriveFinished)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
        vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("g");
    }
    return View(vm);
}

// POST: DriverArea/Drives/Decline/5
/// <summary>
/// Driver area drives POST method decline
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
    var drive = await _appBLL.Drives.DecliningDriveAsync(id, userId, roleName, noTracking:true);
    if (drive == null) return NotFound();

    drive.DriveDeclineDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Declined;
    drive.IsDriveDeclined = true;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();

    var booking = await _appBLL.Bookings.GettingBookingByDriveIdAsync(id, userId, roleName, noTracking:true);
    if (booking != null)
    {
        booking.StatusOfBooking = StatusOfBooking.Declined;
        booking.DeclineDateAndTime = DateTime.UtcNow;
        booking.IsDeclined = true;
        booking.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Bookings.Update(booking);
        await _appBLL.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

// GET: DriverArea/Drives/StartDrive/5
/// <summary>
/// Driver area drives GET method start drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> StartDrive(Guid? id)
{
    if (id == null) return NotFound();

    var vm = new DriveStateViewModel();
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value, userId, roleName);
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime; 
    drive.Booking.Schedule!.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    if (vm.IsDriveAccepted )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
    }
    
    if (vm.IsDriveDeclined )
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("g");
    }

    if (vm.IsDriveStarted)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
    }

    if (vm.IsDriveFinished)
    {
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");
        vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("g");
    }

    return View(vm);
}

// POST: DriverArea/Bookings/Start/5
/// <summary>
/// Driver area drives POST method start drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(StartDrive))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> StartConfirmed(Guid id)
{
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    
    var drive = await _appBLL.Drives.StartingDriveAsync(id, userId, roleName);
    if (drive == null) return NotFound();

    drive.DriveStartDateAndTime = DateTime.Now.ToUniversalTime();
    drive.StatusOfDrive = StatusOfDrive.Started;
    drive.IsDriveStarted = true;
    drive.UpdatedAt = DateTime.Now.ToUniversalTime();

    _appBLL.Drives.Update(drive);
    await _appBLL.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

// GET: DriverArea/Drives/EndDrive/5
/// <summary>
/// Driver area drives GET method end drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>View</returns>
public async Task<IActionResult> EndDrive(Guid? id)
{
    if (id == null) return NotFound();
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    var vm = new DriveStateViewModel();
    var drive = await _appBLL.Drives.GettingFirstDriveAsync(id.Value,userId, roleName );
    if (drive == null) return NotFound();

    vm.Id = drive.Id;
    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime; 
    drive.Booking.Schedule!.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime;
    vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
    vm.City = drive.Booking.City!.CityName;
    vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
    vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
    vm.DestinationAddress = drive.Booking.DestinationAddress;
    vm.PickupAddress = drive.Booking.PickupAddress;
    vm.VehicleType = drive.Booking.Vehicle.VehicleType!.VehicleTypeName;
    vm.HasAnAssistant = drive.Booking.HasAnAssistant;
    vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
    vm.StatusOfBooking = drive.Booking.StatusOfBooking;
    vm.StatusOfDrive = drive.StatusOfDrive;
    vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
    if (drive.IsDriveAccepted)
        vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToString("g");
    if (drive.IsDriveDeclined)
        vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToString("g");

    if (drive.IsDriveStarted)
        vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToString("g");

    if (drive.IsDriveFinished) 
        vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToString("g");

    return View(vm);
}

// POST: AdminArea/Drives/EndDrive/5
/// <summary>
/// Driver area drives POST method end drive
/// </summary>
/// <param name="id">Id</param>
/// <returns>Redirect to index</returns>
[HttpPost]
[ActionName(nameof(EndDrive))]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EndDriveConfirmed(Guid id)
{
    var userId = User.GettingUserId();
    var roleName = User.GettingUserRoleName();
    
    var drive = await _appBLL.Drives.EndingDriveAsync(id, userId, roleName);
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