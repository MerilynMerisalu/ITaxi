#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area schedules controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class SchedulesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area schedules controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public SchedulesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Schedules
    /// <summary>
    /// Admin area schedules controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Schedules
            .GettingAllOrderedSchedulesWithIncludesAsync(null, null);
        
        return View(res);
    }

    // GET: AdminArea/Schedules/Details/5
    /// <summary>
    /// Admin area schedules controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt;
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt;

        return View(vm);
    }
    
    /// <summary>
    /// Admin area schedules controller set drop down list
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromRoute]Guid id)
    {
        var vm = new CreateScheduleViewModel();
        var vehicles = await _appBLL.Vehicles.GettingVehiclesByDriverIdAsync(id);
        vm.Vehicles = new SelectList(vehicles, nameof(VehicleDTO.Id),
            nameof(VehicleDTO.VehicleIdentifier));
        return Ok(vm);
    }

    // GET: AdminArea/Schedules/Create
    /// <summary>
    /// Admin area schedules controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateScheduleViewModel();
        var roleName = User.GettingUserRoleName();
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id), $"{nameof(DriverDTO.AppUser)}.{nameof(DriverDTO.AppUser.LastAndFirstName)}");
        vm.Vehicles = new SelectList(await _appBLL.Vehicles.GettingOrderedVehiclesAsync(),
            nameof(VehicleDTO.Id), nameof(VehicleDTO.VehicleIdentifier));

        return View(vm);
    }

    // POST: AdminArea/Schedules/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area schedules controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="schedule">Schedule</param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateScheduleViewModel vm, ScheduleDTO schedule)
    {
        if (ModelState.IsValid)
        {
            schedule.Id = Guid.NewGuid();
            schedule.DriverId = vm.DriverId;
            schedule.StartDateAndTime = DateTime.Parse(vm.StartDateAndTime).ToUniversalTime();
            schedule.EndDateAndTime = DateTime.Parse(vm.EndDateAndTime).ToUniversalTime();
            schedule.CreatedBy = User.Identity!.Name;
            schedule.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Schedules.Add(schedule);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }
    
    // GET: AdminArea/Schedules/Delete/5
    /// <summary>
    /// Admin area schedules controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteScheduleViewModel();
        if (id == null) return NotFound();
        
        var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(id.Value);
        if (schedule == null) return NotFound();

        vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
        vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
        vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
        vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");
        vm.CreatedBy = schedule.CreatedBy!;
        vm.CreatedAt = schedule.CreatedAt;
        vm.UpdatedBy = schedule.UpdatedBy!;
        vm.UpdatedAt = schedule.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/Schedules/Delete/5
    /// <summary>
    /// Admin area schedules controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var schedule = await _appBLL.Schedules.FirstOrDefaultAsync(id);
        if (await _appBLL.RideTimes.HasScheduleAnyAsync(id) || 
            await _appBLL.Bookings.HasAnyScheduleAsync(id))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (schedule != null) _appBLL.Schedules.Remove(schedule);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(Guid id)
    {
        return _appBLL.Schedules.Exists(id);
    }
}