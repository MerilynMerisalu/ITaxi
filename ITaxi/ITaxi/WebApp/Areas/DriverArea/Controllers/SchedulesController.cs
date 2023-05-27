using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
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
    private readonly IAppBLL _appBLL;

    public SchedulesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: DriverArea/Schedules
    public async Task<IActionResult> Index()
    {
#warning Should this be a repository method

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        

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
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);

        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;

#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");

        return View(vm);
    }


    // GET: DriverArea/Schedules/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateScheduleViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
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
    public async Task<IActionResult> Create(CreateScheduleViewModel vm, ScheduleDTO schedule)
    {
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            var driver = await _appBLL.Drivers.GettingDriverByAppUserIdAsync(userId);
            
            schedule.Id = Guid.NewGuid();
            schedule.DriverId = driver.Id;
            schedule.VehicleId = vm.VehicleId;
            schedule.StartDateAndTime = DateTime.Parse(vm.StartDateAndTime);
            schedule.EndDateAndTime = DateTime.Parse(vm.EndDateAndTime);
            schedule.CreatedBy = User.Identity!.Name;
            schedule.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Schedules.Add(schedule);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier),
            nameof(schedule.VehicleId));

        return View(vm);
    }

    // GET: DriverArea/Schedules/Edit/5
    /*public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new EditScheduleViewModel();
        if (id == null) return NotFound();

        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;

        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName),
            nameof(Vehicle.Id),
            nameof(Vehicle.VehicleIdentifier));
        vm.StartDateAndTime = DateTime.Parse(schedule.StartDateAndTime.ToString("g"));
        vm.EndDateAndTime = DateTime.Parse(schedule.EndDateAndTime.ToString("g"))
            ;
        vm.VehicleId = schedule.VehicleId;

        return View(vm);
    }
    */

    // POST: DriverArea/Schedules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditScheduleViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName );
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
                _appBLL.Schedules.Update(schedule);
                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(schedule.Vehicle.Id), nameof(schedule.Vehicle.VehicleIdentifier),
            nameof(schedule.VehicleId));
        return View(vm);
    }*/

    // GET: DriverArea/Schedules/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value,userId, roleName);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");

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
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id, userId, roleName, noTracking: true);
        if (await _appBLL.RideTimes.HasScheduleAnyAsync(id) || await _appBLL.Bookings.HasAnyScheduleAsync(id))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (schedule != null)
        {
            await _appBLL.Schedules.RemoveAsync(schedule.Id);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}