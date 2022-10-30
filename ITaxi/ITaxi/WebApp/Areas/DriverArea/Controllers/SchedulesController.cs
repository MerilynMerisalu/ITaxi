using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.DriverArea.ViewModels;

namespace WebApp.Areas.DriverArea.Controllers;

[Area(nameof(DriverArea))]
[Authorize(Roles = "Admin, Driver")]
public class SchedulesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public SchedulesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: DriverArea/Schedules
    public async Task<IActionResult> Index()
    {
#warning Should this be a repository method

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        foreach (var s in res)
        {
            s.StartDateAndTime = s.StartDateAndTime.ToLocalTime();
            s.EndDateAndTime = s.EndDateAndTime.ToLocalTime();
            s.CreatedAt = s.CreatedAt.ToLocalTime();
            s.UpdatedAt = s.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: DriverArea/Schedules/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
#warning create a custom 404 (Not found) page
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);

        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;

#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime().ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime().ToString("g");

        return View(vm);
    }


    // GET: DriverArea/Schedules/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateScheduleViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier));
#warning Schedule StartDateAndTime needs a custom validation

#warning Schedule EndDateAndTime needs a custom validation

        return View(vm);
    }

    // POST: DriverArea/Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateScheduleViewModel vm, Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            var driverId = _uow.Drivers
                .SingleOrDefaultAsync(d => d!.AppUserId.Equals(userId)).Result!.Id;
            schedule.Id = Guid.NewGuid();
            schedule.DriverId = driverId;
            schedule.VehicleId = vm.VehicleId;
            schedule.StartDateAndTime = DateTime.Parse(vm.StartDateAndTime).ToUniversalTime();
            schedule.EndDateAndTime = DateTime.Parse(vm.EndDateAndTime).ToUniversalTime();
            schedule.CreatedBy = User.Identity!.Name;
            schedule.CreatedAt = DateTime.Now.ToUniversalTime();
            _uow.Schedules.Add(schedule);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier),
            nameof(schedule.VehicleId));

        return View(vm);
    }

    // GET: DriverArea/Schedules/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new EditScheduleViewModel();
        if (id == null) return NotFound();

        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;

        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
            nameof(Vehicle.Id),
            nameof(Vehicle.VehicleIdentifier));
        vm.StartDateAndTime = DateTime.Parse(schedule.StartDateAndTime.ToString("g")).ToLocalTime();
        vm.EndDateAndTime = DateTime.Parse(schedule.EndDateAndTime.ToString("g"))
            .ToLocalTime();
        vm.VehicleId = schedule.VehicleId;

        return View(vm);
    }

    // POST: DriverArea/Schedules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditScheduleViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName );
        if (schedule == null || schedule.Id != id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                schedule.Id = id;
                schedule.VehicleId = vm.VehicleId;
                schedule.StartDateAndTime = vm.StartDateAndTime.ToUniversalTime();
                schedule.EndDateAndTime = vm.EndDateAndTime.ToUniversalTime();
                schedule.UpdatedBy = User.Identity!.Name;
                schedule.UpdatedAt = DateTime.Now.ToUniversalTime();
                _uow.Schedules.Update(schedule);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(schedule.Vehicle.Id), nameof(schedule.Vehicle.VehicleIdentifier),
            nameof(schedule.VehicleId));
        return View(vm);
    }

    // GET: DriverArea/Schedules/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime().ToString("g");
        vm.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime().ToString("g");

        return View(vm);
    }

    // POST: DriverArea/Schedules/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName);
        if (await _uow.RideTimes.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id))
            || await _uow.Bookings.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (schedule != null)
        {
            _uow.Schedules.Remove(schedule);
            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(Guid id)
    {
        return _uow.Schedules.Exists(id);
    }
}