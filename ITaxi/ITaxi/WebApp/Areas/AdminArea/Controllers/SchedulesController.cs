#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class SchedulesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public SchedulesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Schedules
    public async Task<IActionResult> Index()
    {
#warning Ask if this is the right way to get the user name of a logged in user
#warning Ask how to get the user role using interface
#warning Should this be a repository method
        var res = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(null, null);
        foreach (var s in res)
        {
            s.StartDateAndTime = s.StartDateAndTime.ToLocalTime();
            s.CreatedAt = s.CreatedAt.ToLocalTime();
            s.EndDateAndTime = s.EndDateAndTime.ToLocalTime();
            s.UpdatedAt = s.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Schedules/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();

        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id.Value, null, roleName);

        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime().ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }
    
    [HttpPost]
    public async Task<IActionResult> SetDropDownList(Guid id)
    {
        var vm = new CreateScheduleViewModel();
        var vehicles = await _uow.Vehicles.GettingVehiclesByDriverIdAsync(id);
        vm.Vehicles = new SelectList(vehicles, nameof(Vehicle.Id),
            nameof(Vehicle.VehicleIdentifier));
        return Ok(vm);

    }

    // GET: AdminArea/Schedules/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateScheduleViewModel();
        var roleName = User.GettingUserRoleName();
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), $"{nameof(Driver.AppUser)}.{nameof(Driver.AppUser.LastAndFirstName)}");
        vm.Vehicles = new SelectList(new Vehicle[0],
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier));
#warning Schedule StartDateAndTime needs a custom validation

#warning Schedule EndDateAndTime needs a custom validation

        return View(vm);
    }

    // POST: AdminArea/Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateScheduleViewModel vm, Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            schedule.Id = Guid.NewGuid();
            schedule.DriverId = vm.DriverId;
#warning Should this be a repository method
            schedule.StartDateAndTime = DateTime.Parse(vm.StartDateAndTime).ToUniversalTime();
#warning Should this be a repository method
            schedule.EndDateAndTime = DateTime.Parse(vm.EndDateAndTime).ToUniversalTime();
            schedule.CreatedBy = User.Identity!.Name;
            schedule.CreatedAt = DateTime.Now.ToUniversalTime();
            _uow.Schedules.Add(schedule);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Schedules/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new EditScheduleViewModel();
        if (id == null) return NotFound();

        var schedule = await _uow.Schedules.FirstOrDefaultAsync(id.Value);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;

        vm.DriverId = schedule.DriverId;
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(null, roleName),
            nameof(Vehicle.Id),
            nameof(Vehicle.VehicleIdentifier));
        vm.StartDateAndTime = DateTime.Parse(schedule.StartDateAndTime.ToString("g")).ToLocalTime();
        vm.EndDateAndTime = DateTime.Parse(schedule.EndDateAndTime.ToString("g"))
            .ToLocalTime();
        vm.VehicleId = schedule.VehicleId;
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), "AppUser.LastAndFirstName");

        return View(vm);
    }

    // POST: AdminArea/Schedules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditScheduleViewModel vm)
    {
        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id, null, roleName);

        if (schedule != null && id != schedule.Id) return NotFound();


        if (ModelState.IsValid)
        {
            try
            {
                if (schedule != null)
                {
                    schedule.Id = id;

                    schedule.DriverId = vm.DriverId;
                    schedule.VehicleId = vm.VehicleId;
#warning Should this be a repository method
                    schedule.StartDateAndTime = vm.StartDateAndTime.ToUniversalTime();
#warning Should this be a repository method
                    schedule.EndDateAndTime = vm.EndDateAndTime.ToUniversalTime();
                    schedule.UpdatedAt = DateTime.Now.ToUniversalTime();
                    schedule.UpdatedBy = User.Identity!.Name;


                    _uow.Schedules.Update(schedule);
                    await _uow.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (schedule != null && !ScheduleExists(schedule.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Schedules/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();


        var roleName = User.GettingUserRoleName();
        var schedule = await _uow.Schedules.GettingTheFirstScheduleByIdAsync(id.Value, null, roleName);
        if (schedule == null) return NotFound();

        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime().ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime().ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt.ToLocalTime().ToString("G");


        return View(vm);
    }

    // POST: AdminArea/Schedules/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var schedule = await _uow.Schedules.FirstOrDefaultAsync(id);
        if (await _uow.RideTimes.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id))
            || await _uow.Bookings.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (schedule != null) _uow.Schedules.Remove(schedule);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(Guid id)
    {
        return _uow.Schedules.Exists(id);
    }
}