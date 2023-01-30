#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;


namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class SchedulesController : Controller
{
    private readonly IAppBLL _appBLL;

    public SchedulesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Schedules
    public async Task<IActionResult> Index()
    {

        var res = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(null, null);
        
        return View(res);
    }

    // GET: AdminArea/Schedules/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();

        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value );

        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;
        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt;
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt;

        return View(vm);
    }
    
    [HttpPost]
    public async Task<IActionResult> SetDropDownList(Guid id)
    {
        var vm = new CreateScheduleViewModel();
        var vehicles = await _appBLL.Vehicles.GettingVehiclesByDriverIdAsync(id);
        vm.Vehicles = new SelectList(vehicles, nameof(VehicleDTO.Id),
            nameof(VehicleDTO.VehicleIdentifier));
        return Ok(vm);

    }

    // GET: AdminArea/Schedules/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateScheduleViewModel();
        var roleName = User.GettingUserRoleName();
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), $"{nameof(DriverDTO.AppUser)}.{nameof(DriverDTO.AppUser.LastAndFirstName)}");
        /*vm.Vehicles = new SelectList(new VehicleDTO[0],
            nameof(VehicleDTO.Id), nameof(VehicleDTO.VehicleIdentifier));*/
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(VehicleDTO.Id), nameof(VehicleDTO.VehicleIdentifier));
#warning Schedule StartDateAndTime needs a custom validation

#warning Schedule EndDateAndTime needs a custom validation

        return View(vm);
    }

    // POST: AdminArea/Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateScheduleViewModel vm, ScheduleDTO schedule)
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
            _appBLL.Schedules.Add(schedule);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Schedules/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        
        var vm = new EditScheduleViewModel();
        if (id == null) return NotFound();

        var schedule = await _appBLL.Schedules.FirstOrDefaultAsync(id.Value);
        if (schedule == null) return NotFound();

        vm.Id = schedule.Id;

        vm.DriverId = schedule.DriverId;
        /*vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingVehiclesByDriverIdAsync(vm.DriverId),
            nameof(VehicleDTO.Id),
            nameof(VehicleDTO.VehicleIdentifier));*/
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(VehicleDTO.Id), nameof(VehicleDTO.VehicleIdentifier));
        vm.StartDateAndTime = DateTime.Parse(schedule.StartDateAndTime.ToString("g"));
        vm.EndDateAndTime = DateTime.Parse(schedule.EndDateAndTime.ToString("g"));
        vm.VehicleId = schedule.VehicleId;
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");

        return View(vm);
    }

    // POST: AdminArea/Schedules/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditScheduleViewModel vm)
    {
        
        var schedule = await _appBLL.Schedules.FirstOrDefaultAsync(id, true, true);

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
                    schedule.StartDateAndTime = vm.StartDateAndTime;
#warning Should this be a repository method
                    schedule.EndDateAndTime = vm.EndDateAndTime;
                    schedule.UpdatedAt = DateTime.Now;
                    schedule.UpdatedBy = User.Identity!.Name;


                    _appBLL.Schedules.Update(schedule);
                    await _appBLL.SaveChangesAsync();
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
        
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value);
        if (schedule == null) return NotFound();

        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
#warning Should this be a repository method
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
#warning Should this be a repository method
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt;
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt;


        return View(vm);
    }

    // POST: AdminArea/Schedules/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var schedule = await _appBLL.Schedules.FirstOrDefaultAsync(id);
        /*if (await _appBLL.RideTimes.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id))
            || await _appBLL.Bookings.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");*/

        if (schedule != null) _appBLL.Schedules.Remove(schedule);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}