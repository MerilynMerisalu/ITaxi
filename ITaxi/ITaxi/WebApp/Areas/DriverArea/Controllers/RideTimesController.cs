using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.DriverArea.ViewModels;

namespace WebApp.Areas.DriverArea.Controllers;

/// <summary>
/// Driver area ride times controller
/// </summary>
[Area(nameof(DriverArea))]
[Authorize(Roles = "Admin, Driver")]
public class RideTimesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Driver area ride times controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public RideTimesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: DriverArea/RideTimes
    /// <summary>
    /// Driver area ride times GET method index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.RideTimes.GettingAllOrderedRideTimesAsync(userId, roleName);
        
        return View(res);
    }

    // GET: DriverArea/RideTimes/Details/5
    /// <summary>
    /// Driver area ride times GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
        vm.Id = rideTime.Id;

        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;

        return View(vm);
    }

    // GET: DriverArea/RideTimes/Create
    /// <summary>
    /// Driver area ride times GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateRideTimeViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        vm.Schedules = new SelectList(
            await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName)
            , nameof(ScheduleDTO.Id), nameof(ScheduleDTO.ShiftDurationTime));
        var schedules = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
 
        vm.RideTimes = new SelectList(new string[0]);
        
        return View(vm);
    }
    
    /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    /// <param name="id">The Id (Guid) of the selected RideTime</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromRoute] Guid id)
    {
        await Task.CompletedTask;
        // Using the CreateRideTimeViewModel because I want to send through the SelectLists and Ids that have now changed
        var vm = new CreateRideTimeViewModel();
        vm.ScheduleId = id;

        // Always refreshing the RideTimes, if the DriverId or the ScheduleId are changed
        // => because the ScheduleId is ALWAYS changed when the DriverId is changed.
        // Select the RideTimes form the currently selected schedule, for the current driver
        var rideTimes = _appBLL.RideTimes.GettingRemainingRideTimesByScheduleId(vm.ScheduleId);

        // The times in schedules have already been converted!
        vm.RideTimes = new SelectList(rideTimes.Select(x => new {RideTime = x}),
            "RideTime", "RideTime");

        return Ok(vm);
    }

    // POST: DriverArea/RideTimes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Driver area ride times POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRideTimeViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var driver = await _appBLL.Drivers.GettingDriverByAppUserIdAsync(userId);
        if (ModelState.IsValid)
        {
            if (vm.SelectedRideTimes != null)
            {
                foreach (var selectedRideTime in vm.SelectedRideTimes)
                    if (driver != null)
                    {
                        var rideTime = new RideTimeDTO()
                        {
                            Id = new Guid(),
                            DriverId = driver.Id,
                            ScheduleId = vm.ScheduleId,
                            RideDateTime = selectedRideTime.ToUniversalTime(),
                            IsTaken = vm.IsTaken,
                            CreatedBy = User.Identity!.Name,
                            CreatedAt = DateTime.Now.ToUniversalTime()
                        };

                        _appBLL.RideTimes.Add(rideTime);
                    }

                await _appBLL.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        
        vm.Schedules = new SelectList(await _appBLL.Schedules
                .GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName),
            nameof(ScheduleDTO.Id),
            nameof(ScheduleDTO.ShiftDurationTime));

        return View(vm);
    }

    // GET: DriverArea/RideTimes/Edit/5
    /// <summary>
    /// Driver area ride times GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new EditRideTimeViewModel();
        if (id == null) return NotFound();

        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
        vm.Schedules = new SelectList(
            await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName),
            nameof(ScheduleDTO.Id), nameof(ScheduleDTO.ShiftDurationTime));
        var schedules = await _appBLL.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        vm.IsTaken = rideTime.IsTaken;

        var rideTimes = _appBLL.RideTimes.CalculatingRideTimes(rideTime.ScheduleId);
        var rideTimeList = new List<string>();
        foreach (var rideTimeLocal in rideTimes)
            rideTimeList.Add(DateTime.Parse(rideTimeLocal).ToString("t"));

        vm.RideTimes = new SelectList(rideTimeList);
        vm.ScheduleId = rideTime.ScheduleId;
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        return View(vm);
    }
    
    // POST: DriverArea/RideTimes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Driver area ride times POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditRideTimeViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);

        if (id != rideTime!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                rideTime.Id = id;
                rideTime.ScheduleId = vm.ScheduleId;
                if (vm.RideTime != null) rideTime.RideDateTime = DateTime.Parse(vm.RideTime).ToUniversalTime();
                rideTime.IsTaken = vm.IsTaken;
                rideTime.UpdatedBy = User.Identity!.Name;
                rideTime.UpdatedAt = DateTime.Now.ToUniversalTime();

                _appBLL.RideTimes.Update(rideTime);
                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideTimeExists(rideTime.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: DriverArea/RideTimes/Delete/5
    /// <summary>
    /// Driver area ride times GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();

        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;

        return View(vm);
    }
    
    // POST: DriverArea/RideTimes/Delete/5
    /// <summary>
    /// Driver area ride times POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var rideTime = await _appBLL.RideTimes.FirstOrDefaultAsync(id);
        if (rideTime != null) await _appBLL.RideTimes.RemoveAsync(rideTime.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    private bool RideTimeExists(Guid id)
    {
        return _appBLL.RideTimes.Exists(id);
    }
}