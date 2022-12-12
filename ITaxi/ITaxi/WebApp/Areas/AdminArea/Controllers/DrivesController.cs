#nullable enable
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class DrivesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public DrivesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Drives
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Drives.GettingAllOrderedDrivesWithIncludesAsync(null, roleName);
#warning Should this be a repo method
        foreach (var drive in res)
        {
            drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime.ToLocalTime();
            drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime.ToLocalTime();
            drive.Booking.PickUpDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime();
            
            if (drive.IsDriveAccepted || (drive.IsDriveAccepted && drive.IsDriveDeclined))
                drive.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime();

            if (drive.IsDriveDeclined || (drive.IsDriveAccepted && drive.IsDriveDeclined))
                drive.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToLocalTime();

            if (drive.IsDriveStarted || (drive.IsDriveAccepted && drive.IsDriveStarted))
                drive.DriveStartDateAndTime = drive.DriveStartDateAndTime.ToLocalTime();

            if (drive.IsDriveFinished) drive.DriveEndDateAndTime = drive.DriveEndDateAndTime.ToLocalTime();

            drive.CreatedAt = drive.CreatedAt.ToLocalTime();
            drive.UpdatedAt = drive.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Drives/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new DetailsDriveViewModel();
        if (id == null) return NotFound();

        var drive = await _uow.Drives
            .GettingFirstDriveAsync(id.Value, null, roleName);
        if (drive == null) return NotFound();

        vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
        vm.City = drive.Booking!.City!.CityName;
        drive.Booking.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime.ToLocalTime();
        drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime.ToLocalTime();
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        if (drive.Comment?.CommentText != null) vm.CommentText = drive.Comment.CommentText;

        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("G");
        
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        vm.IsDriveAccepted = drive.IsDriveAccepted;
        vm.IsDriveDeclined = drive.IsDriveDeclined;
        vm.IsDriveStarted = drive.IsDriveStarted;
        vm.IsDriveFinished = drive.IsDriveFinished;
        if (drive.IsDriveAccepted)
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime().ToString("G");

        if (drive.IsDriveDeclined)
            vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToLocalTime().ToString("G");

        if (drive.IsDriveStarted)
            vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToLocalTime().ToString("G");

        if (drive.IsDriveFinished) vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToLocalTime().ToString("G");


        vm.CreatedBy = drive.CreatedBy!;
        vm.CreatedAt = drive.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = drive.UpdatedBy!;
        vm.UpdatedAt = drive.UpdatedAt.ToLocalTime().ToString("G");


        return View(vm);
    }

    /*
    // GET: AdminArea/Drives/Create
    public IActionResult Create()
    {
        ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address");
        return View();
    }

    // POST: AdminArea/Drives/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
    {
        if (ModelState.IsValid)
        {
            drive.Id = Guid.NewGuid();
            _uow.Add(drive);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
        return View(drive);
    }

    // GET: AdminArea/Drives/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var drive = await _uow.Drives.FindAsync(id);
        if (drive == null)
        {
            return NotFound();
        }
        ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
        return View(drive);
    }

    // POST: AdminArea/Drives/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
    {
        if (id != drive.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _uow.Update(drive);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriveExists(drive.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
        return View(drive);
    }

    // GET: AdminArea/Drives/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var drive = await _uow.Drives
            .Include(d => d.Driver)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (drive == null)
        {
            return NotFound();
        }

        return View(drive);
    }

    // POST: AdminArea/Drives/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var drive = await _uow.Drives.FindAsync(id);
        _uow.Drives.Remove(drive);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    */

    /// <summary>
    ///     Search drives by inserted date
    /// </summary>
    /// <param name="search">date</param>
    /// <returns>An index view with search results</returns>
    [HttpPost]
    public async Task<IActionResult> SearchByDateAsync([FromForm] DateTime search)
    {
        var roleName = User.GettingUserRoleName();
        var drives = await _uow.Drives.SearchByDateAsync(search, null, roleName);
        return View(nameof(Index), drives);
    }

    /// <summary>
    ///     Generates a pdf view of drives
    /// </summary>
    /// <returns>A pdf of drives</returns>
    public async Task<IActionResult> Print()
    {
        var roleName = User.GettingUserRoleName();
        
        
            var drives = await _uow.Drives.PrintAsync( null, roleName );
            foreach (var drive in drives)
            {
                if (drive != null)
                {
                    drive.Booking!.Schedule!.StartDateAndTime = drive.Booking.Schedule.StartDateAndTime.ToLocalTime();
                    drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime.ToLocalTime();
                    drive.Booking.PickUpDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime();
                }
            }

            return new ViewAsPdf("PrintDrives", drives);
        

        
    }

    // GET: AdminArea/Drives/Accept/5
    public async Task<IActionResult> Accept(Guid? id)
    {
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var vm = new DriveStateViewModel();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
        if (drive == null) return NotFound();

        vm.Id = drive.Id;
        drive.Booking!.Schedule!.StartDateAndTime = drive.Booking!.Schedule!.StartDateAndTime.ToLocalTime();
        drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime.ToLocalTime();
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        vm.City = drive.Booking.City!.CityName;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("G");
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        if (vm.IsDriveAccepted )
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime().ToString("G");
        }
        if (vm.IsDriveDeclined )
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime().ToString("G");
            vm.DriveDeclineDateAndTime = drive.DriveDeclineDateAndTime.ToLocalTime().ToString("G");
        }

        if (vm.IsDriveStarted)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime().ToString("G");
            vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToLocalTime().ToString("G");
        }

        if (vm.IsDriveFinished)
        {
            vm.DriveAcceptedDateAndTime = drive.DriveAcceptedDateAndTime.ToLocalTime().ToString("G");
            vm.DriveInProgressDateAndTime = drive.DriveStartDateAndTime.ToLocalTime().ToString("G");
            vm.DriveFinishedDateAndTime = drive.DriveEndDateAndTime.ToLocalTime().ToString("G");
        }

        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = drive.CreatedBy!;
        vm.CreatedAt = drive.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = drive.UpdatedBy!;
        vm.UpdatedAt = drive.UpdatedAt.ToLocalTime().ToString("G");


        return View(vm);
    }

    // POST: AdminArea/Bookings/Accept/5
    [HttpPost]
    [ActionName(nameof(Accept))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive = await _uow.Drives.AcceptingDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive.DriveAcceptedDateAndTime = DateTime.Now.ToUniversalTime();
        drive.StatusOfDrive = StatusOfDrive.Accepted;
        drive.IsDriveAccepted = true;
        drive.UpdatedAt = DateTime.Now.ToUniversalTime();

        _uow.Drives.Update(drive);
        await _uow.SaveChangesAsync();

        var booking = await _uow.Bookings.SingleOrDefaultAsync(b => b!.DriveId.Equals(drive.Id), false);
        if (booking != null)
        {
            booking.StatusOfBooking = StatusOfBooking.Accepted;
            var bestRideTime = await _uow.RideTimes.GettingBestAvailableRideTimeAsync(booking.PickUpDateAndTime, booking.CityId, booking.NumberOfPassengers, booking.VehicleTypeId, false, null, null, false);
            //bestRideTime.Single().IsTaken = true;
            
                
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.Bookings.Update(booking);
            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: AdminArea/Drives/Decline/5
    public async Task<IActionResult> Decline(Guid? id)
    {
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var vm = new DriveStateViewModel();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
        if (drive == null) return NotFound();

        vm.Id = drive.Id;
        drive.Booking!.Schedule!.StartDateAndTime = drive.Booking!.Schedule!.StartDateAndTime.ToLocalTime();
        drive.Booking.Schedule.EndDateAndTime = drive.Booking.Schedule.EndDateAndTime.ToLocalTime();
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        vm.City = drive.Booking.City!.CityName;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("G");
        vm.CreatedBy = drive.CreatedBy!;
        vm.CreatedAt = drive.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = drive.UpdatedBy!;
        vm.UpdatedAt = drive.UpdatedAt.ToLocalTime().ToString("G");


        return View(vm);
    }

    // POST: AdminArea/Drives/Decline/5
    [HttpPost]
    [ActionName(nameof(Decline))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive = await _uow.Drives.DecliningDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive.DriveDeclineDateAndTime = DateTime.Now.ToUniversalTime();
        drive.StatusOfDrive = StatusOfDrive.Declined;
        drive.IsDriveDeclined = true;
        drive.UpdatedBy = User.Identity!.Name;
        drive.UpdatedAt = DateTime.Now.ToUniversalTime();

        _uow.Drives.Update(drive);
        await _uow.SaveChangesAsync();

        var booking = await _uow.Bookings.SingleOrDefaultAsync(b => b!.DriveId.Equals(drive.Id), 
            false);
        if (booking != null)
        {
            booking.StatusOfBooking = StatusOfBooking.Declined;
            booking.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.Bookings.Update(booking);
            
            var rideTime = await _uow.RideTimes.GettingFirstRideTimeByBookingIdAsync(booking.Id, null, null, false);
            if (rideTime != null)
            {
                rideTime.BookingId = null;
                rideTime.ExpiryTime = null;
                rideTime.IsTaken = false;
                _uow.RideTimes.Update(rideTime);
            }

            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: AdminArea/Drives/StartDrive/5
    public async Task<IActionResult> StartDrive(Guid? id)
    {
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();
        var vm = new DriveStateViewModel();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
        if (drive == null) return NotFound();

        vm.Id = drive.Id;
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        vm.City = drive.Booking.City!.CityName;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = drive.CreatedBy!;
        vm.CreatedAt = drive.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = drive.UpdatedBy!;
        vm.UpdatedAt = drive.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }

    // POST: AdminArea/Bookings/Start/5
    [HttpPost]
    [ActionName(nameof(StartDrive))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive = await _uow.Drives.StartingDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive.DriveStartDateAndTime = DateTime.Now.ToUniversalTime();
        drive.StatusOfDrive = StatusOfDrive.Started;
        drive.IsDriveStarted = true;
        drive.UpdatedAt = DateTime.Now.ToUniversalTime();

        _uow.Drives.Update(drive);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: AdminArea/Drives/EndDrive/5
    public async Task<IActionResult> EndDrive(Guid? id)
    {
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var vm = new DriveStateViewModel();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id.Value, null, roleName);
        if (drive == null) return NotFound();

        vm.Id = drive.Id;
        vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
        vm.City = drive.Booking.City!.CityName;
        vm.CustomerLastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
        vm.DriverLastAndFirstName = drive.Driver!.AppUser!.LastAndFirstName;
        vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
        vm.DestinationAddress = drive.Booking.DestinationAddress;
        vm.PickupAddress = drive.Booking.PickupAddress;
        vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = drive.Booking.HasAnAssistant;
        vm.NumberOfPassengers = drive.Booking.NumberOfPassengers;
        vm.StatusOfBooking = drive.Booking.StatusOfBooking;
        vm.StatusOfDrive = drive.StatusOfDrive;
        vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = drive.CreatedBy!;
        vm.CreatedAt = drive.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = drive.UpdatedBy!;
        vm.UpdatedAt = drive.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }


    // POST: AdminArea/Drives/EndDrive/5
    [HttpPost]
    [ActionName(nameof(EndDrive))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndDriveConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive = await _uow.Drives.EndingDriveAsync(id, null, roleName);
        if (drive == null) return NotFound();

        drive.IsDriveFinished = true;
        drive.StatusOfDrive = StatusOfDrive.Finished;
        drive.DriveEndDateAndTime = DateTime.Now.ToUniversalTime();
        drive.UpdatedAt = DateTime.Now.ToUniversalTime();

        _uow.Drives.Update(drive);
        await _uow.SaveChangesAsync();


        return RedirectToAction(nameof(Index));
    }
}