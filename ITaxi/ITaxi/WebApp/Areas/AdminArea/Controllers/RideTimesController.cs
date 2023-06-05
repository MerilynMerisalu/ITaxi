#nullable enable

using System.Diagnostics;
using App.Contracts.BLL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.AdminArea.ViewModels;
using DriverDTO = App.BLL.DTO.AdminArea.DriverDTO;
using ScheduleDTO = App.BLL.DTO.AdminArea.ScheduleDTO;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area ride times controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class RideTimesController : Controller
{
    private readonly IAppBLL _appBLL;
    
    /// <summary>
    /// Admin area ride times controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public RideTimesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/RideTimes
    /// <summary>
    /// Admin area ride times controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.RideTimes.GettingAllOrderedRideTimesAsync(null, null);
        
        return View(res);
    }

    // GET: AdminArea/RideTimes/Details/5
    /// <summary>
    /// Admin area ride times controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value);
        
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        vm.Driver = rideTime.Schedule!.Driver!.AppUser!.LastAndFirstName;
        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
        vm.RideTime = rideTime.RideDateTime.ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;

        return View(vm);
    }

    // GET: AdminArea/RideTimes/Create
    /// <summary>
    /// Admin area ride times controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var roleName = User.GettingUserRoleName();
        var vm = new CreateRideTimeViewModel();
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");
        vm.Schedules = new SelectList(new Schedule[0]
            , nameof(ScheduleDTO.Id), nameof(Schedule.ShiftDurationTime));
        vm.RideTimes = new SelectList(new string[0]);
    
        return View(vm);
    }

    // POST: AdminArea/RideTimes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area ride times controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRideTimeViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var schedule = await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(vm.ScheduleId);
            if (vm.SelectedRideTimes != null && vm.SelectedRideTimes.Any())
            {
                List<App.BLL.DTO.AdminArea.RideTimeDTO> rideTimes = new();
                foreach (var selectedRideTime in vm.SelectedRideTimes)
                {
                    var rideDateAndTime = schedule!.StartDateAndTime.Date.Add(selectedRideTime.TimeOfDay);
                    if (selectedRideTime.TimeOfDay < schedule.StartDateAndTime.TimeOfDay)
                        rideDateAndTime = rideDateAndTime.AddDays(1);

                    var rideTime = new App.BLL.DTO.AdminArea.RideTimeDTO()
                    {
                        Id = Guid.NewGuid(),
                        DriverId = vm.DriverId,
                        ScheduleId = vm.ScheduleId,
                        RideDateTime = rideDateAndTime.ToUniversalTime(),
                        IsTaken = vm.IsTaken,
                        CreatedBy = User.Identity!.Name,
                        CreatedAt = DateTime.Now.ToUniversalTime()
                    };

                    rideTimes.Add(rideTime);
                    
                }
                await _appBLL.RideTimes.AddRangeAsync(rideTimes);
                await _appBLL.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            // custom validation to check that at least one ride time is chosen
            // This logic has been replicated in RequiredAtLeastOneSelectionAttribute
            // But I leave this code here just in case the attribute is not in place.
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedRideTimes), "Please select at least 1 time");
            }
        }

        // After a Model Error, the VM is reset, so I need to rebuild the 
        // lists so that the user can continue to complete the form
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(Driver.Id), "AppUser.LastAndFirstName");

        if (vm.DriverId != Guid.Empty)
        {
            // I can only set the schedules after the driver has been selected.
            var schedules = await _appBLL.Schedules.GettingTheScheduleByDriverIdAsync(vm.DriverId, null, null);
            foreach (var schedule in schedules)
            {
                schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
                schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
            }

            vm.Schedules = new SelectList(schedules,
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));

            if (vm.ScheduleId != Guid.Empty)
            {
                // Select the RideTimes form the currently selected schedule, for the current driver
                var rideTimesList = _appBLL.RideTimes.GettingRemainingRideTimesByScheduleId(vm.ScheduleId);
                
                // The times in schedules have already been converted!
                vm.RideTimes = new SelectList(rideTimesList.Select(x => new {RideTime = x}), "RideTime", "RideTime");
            }
        }

        return View(vm);
    }

    /// <summary>
    /// Admin area ride times controller set drop down list request
    /// </summary>
    public class SetDropDownListRequest
    {
        /// <summary>
        /// Admin area ride times controller set drop down list request list type
        /// </summary>
        public string? ListType { get; set; }
        /// <summary>
        /// Admin area ride times controller set drop down list request value
        /// </summary>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    
    
    /// <param name="parameters">Parameters</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody] SetDropDownListRequest parameters)
    {
        // Using the EditRideTimeViewModel because I want to send through the SelectLists and Ids that have now changed
        var vm = new EditRideTimeViewModel();
        IEnumerable<ScheduleDTO>? schedules; 

        if (parameters.ListType == nameof(RideTime.DriverId))
        {
            // Refresh the list of schedules for the selected driver
            // The value, represents the current DriverId
            var driverId = Guid.Parse(parameters.Value!);

            schedules = await _appBLL.Schedules.GettingTheScheduleByDriverIdAsync(driverId, null);
 
            //_appBLL.Schedules.
            var scheduleDtos = schedules.ToList();
            vm.Schedules = new SelectList(
                scheduleDtos,
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));

            // Select the first schedule, later if I want to pick the schedule that is closest or overlaps with the previous selection
            vm.ScheduleId = scheduleDtos.FirstOrDefault()!.Id;
        }
        // listType The dropdown list that has been changed
        else if (parameters.ListType == nameof(RideTime.ScheduleId))
        {
            //value The currently selected item value
            if (parameters.Value != null) vm.ScheduleId = Guid.Parse(parameters.Value);

            // reload the schedules, but just the current one so we can rebuild the ride times
            schedules = new[] {await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(vm.ScheduleId, null)}!;

            // NOTE: I do not need to rebuild the SelectList for schedules, only the RideTimes
        }

        // Always refresh the RideTimes, if the DriverId or the ScheduleId are changed
        // => because the ScheduleId is ALWAYS changed when the DriverId is changed.
        // Select the RideTimes form the currently selected schedule, for the current driver
        var rideTimes = _appBLL.RideTimes.GettingRemainingRideTimesByScheduleId(vm.ScheduleId);

        // The times in schedules have already been converted!
        vm.RideTimes = new SelectList(rideTimes.Select(x => new {RideTime = x}),
            nameof(vm.RideTime), nameof(vm.RideTime));

        // I need to select one of these!
        vm.RideTime = rideTimes.First();
        
        return Ok(vm);
    }
    
    // GET: AdminArea/RideTimes/Delete/5
    /// <summary>
    /// Admin area ride times controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();
        
        var rideTime =  await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();

        vm.Driver = rideTime.Schedule.Driver!.AppUser!.LastAndFirstName;
        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;

        return View(vm);
    }

    // POST: AdminArea/RideTimes/Delete/5
    /// <summary>
    /// Admin area ride times controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirection to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id);
        if (rideTime != null && rideTime.IsTaken)
        {
            return Content("Entity cannot be deleted");
        }

        if (rideTime != null) _appBLL.RideTimes.Remove(rideTime);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RideTimeExists(Guid id)
    {
        return _appBLL.RideTimes.Exists(id);
    }
}