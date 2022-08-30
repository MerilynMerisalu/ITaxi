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
public class RideTimesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public RideTimesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: DriverArea/RideTimes
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.RideTimes.GettingAllOrderedRideTimesAsync(userId, roleName);
#warning Should this be a repository method
        foreach (var rideTime in res)
            if (rideTime != null)
            {
                rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
                rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
                rideTime.RideDateTime = rideTime.RideDateTime.ToLocalTime();
            }

        return View(res);
    }

    // GET: DriverArea/RideTimes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
        vm.Id = rideTime.Id;

        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;

        return View(vm);
    }

    // GET: DriverArea/RideTimes/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateRideTimeViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName)
            , nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        var schedules = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        foreach (var schedule in schedules)
        {
            schedule.StartDateAndTime = DateTime.Parse(schedule.StartDateAndTime.ToLocalTime().ToString("g"));
            schedule.EndDateAndTime = DateTime.Parse(schedule.EndDateAndTime.ToLocalTime().ToString("g"));
        }
        var scheduleStartAndEndTime = _uow.Schedules.GettingStartAndEndTime(schedules);
        var rideTimes = _uow.RideTimes.CalculatingRideTimes(scheduleStartAndEndTime);
        vm.RideTimes = new SelectList(rideTimes);


        return View(vm);
    }

    // POST: DriverArea/RideTimes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRideTimeViewModel vm, List<RideTime> rideTimes)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var driver = await _uow.Drivers.SingleOrDefaultAsync(d => d!.AppUserId.Equals(userId));
        if (ModelState.IsValid)
        {
            if (vm.SelectedRideTimes != null)
            {
                foreach (var selectedRideTime in vm.SelectedRideTimes)
                    if (driver != null)
                    {
                        var rideTime = new RideTime
                        {
                            Id = new Guid(),
                            DriverId = driver.Id,
                            ScheduleId = vm.ScheduleId,
                            RideDateTime = selectedRideTime.ToUniversalTime(),
                            IsTaken = vm.IsTaken
                        };

                        rideTimes.Add(rideTime);
                    }

                await _uow.RideTimes.AddRangeAsync(rideTimes);
                await _uow.SaveChangesAsync();
            }
#warning Needs custom validation to check that at least one ride time is chosen

            return RedirectToAction(nameof(Index));
        }


#warning Selectlist of schedules must be recreated when something goes wrong with creating the record
        vm.Schedules = new SelectList(await _uow.Schedules
                .GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName),
            nameof(Schedule.Id),
            nameof(Schedule.ShiftDurationTime));
#warning Selectable ride times must be recreated when something goes wrong with creating the record
#warning Selected ride times remain so when something goes wrong with creating the record
        return View(vm);
    }

    // GET: DriverArea/RideTimes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new EditRideTimeViewModel();
        if (id == null) return NotFound();

        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
        vm.Schedules = new SelectList(
            await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName),
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        var schedules = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName);
        vm.IsTaken = rideTime.IsTaken;
#warning Ridetimes should be hidden and reappearing based on whether IsTaken is true or not
        var rideTimes = _uow.RideTimes.CalculatingRideTimes(_uow.Schedules.GettingStartAndEndTime(schedules));
#warning Ask if there is a better way to implement this
        var rideTimeList = new List<string>();
        foreach (var rideTimeLocal in rideTimes) rideTimeList.Add(DateTime.Parse(rideTimeLocal).ToShortTimeString());

        vm.RideTimes = new SelectList(rideTimeList);
        vm.ScheduleId = rideTime.ScheduleId;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        return View(vm);
    }

    // POST: DriverArea/RideTimes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditRideTimeViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, userId, roleName);

        if (id != rideTime!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                rideTime.Id = id;
                rideTime.ScheduleId = vm.ScheduleId;
                rideTime.RideDateTime = DateTime.Parse(vm.RideTime).ToUniversalTime();
                rideTime.IsTaken = vm.IsTaken;
                rideTime.UpdatedAt = DateTime.Now.ToUniversalTime();

                _uow.RideTimes.Update(rideTime);
                await _uow.SaveChangesAsync();
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
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, userId, roleName);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();

        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;

        return View(vm);
    }


    // POST: DriverArea/RideTimes/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id);
        if (rideTime != null) _uow.RideTimes.Remove(rideTime);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool RideTimeExists(Guid id)
    {
        return _uow.RideTimes.Exists(id);
    }
}