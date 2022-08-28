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
public class RideTimesController : Controller
{
    private readonly IAppUnitOfWork _uow;


    public RideTimesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/RideTimes
    public async Task<IActionResult> Index()
    {
        
        var roleName = User.GettingUserRoleName();
        var res = await _uow.RideTimes.GettingAllOrderedRideTimesAsync(null, roleName);
#warning Should this be a repository method
        foreach (var rideTime in res)
            if (rideTime != null)
            {
                rideTime.Schedule!.StartDateAndTime = rideTime.Schedule!.StartDateAndTime.ToLocalTime();
                rideTime.Schedule!.EndDateAndTime = rideTime.Schedule!.EndDateAndTime.ToLocalTime();
                rideTime.RideDateTime = rideTime.RideDateTime.ToLocalTime();
                rideTime.CreatedAt = rideTime.CreatedAt.ToLocalTime();

                rideTime.UpdatedAt = rideTime.UpdatedAt.ToLocalTime();
            }

        return View(res);
    }

    // GET: AdminArea/RideTimes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();


        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, null, roleName);
        if (rideTime == null) return NotFound();

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();
        
        vm.Id = rideTime.Id;
        vm.Driver = rideTime.Driver!.AppUser!.LastAndFirstName;
        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt.ToString("G");
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt.ToString("G");
        vm.CreatedBy = rideTime.CreatedBy!;

        return View(vm);
    }

    // GET: AdminArea/RideTimes/Create
    public async Task<IActionResult> Create()
    {
        var roleName = User.GettingUserRoleName();
        var vm = new CreateRideTimeViewModel();
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), "AppUser.LastAndFirstName");
        vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(null, roleName)
            , nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        var schedules = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(null, roleName);
        var scheduleStartAndEndTime = _uow.Schedules.GettingStartAndEndTime(schedules);
        var rideTimes = _uow.RideTimes.CalculatingRideTimes(scheduleStartAndEndTime);
        vm.RideTimes = new SelectList(rideTimes);


        return View(vm);
    }

    // POST: AdminArea/RideTimes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRideTimeViewModel vm, List<RideTime> rideTimes)
    {
        if (ModelState.IsValid)
        {
            if (vm.SelectedRideTimes != null)
            {
                foreach (var selectedRideTime in vm.SelectedRideTimes)
                {
                    var rideTime = new RideTime
                    {
                        Id = new Guid(),
                        DriverId = vm.DriverId,
                        ScheduleId = vm.ScheduleId,
                        RideDateTime = selectedRideTime.ToUniversalTime(),
                        IsTaken = vm.IsTaken,
                        CreatedBy = User.Identity!.Name,
                        CreatedAt = DateTime.Now.ToUniversalTime()
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
        vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(),
            nameof(Schedule.Id),
            nameof(Schedule.ShiftDurationTime));
#warning Selectable ride times must be recreated when something goes wrong with creating the record
#warning Selected ride times remain so when something goes wrong with creating the record
        return View(vm);
    }

    // GET: AdminArea/RideTimes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new EditRideTimeViewModel();
        if (id == null) return NotFound();

        var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id.Value);
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        vm.DriverId = rideTime.DriverId;
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), "AppUser.LastAndFirstName");
        vm.Schedules = new SelectList(
            await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(),
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        var schedules = await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(null, roleName);
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

    // POST: AdminArea/RideTimes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditRideTimeViewModel vm)
    {
        
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, null, roleName);

        if (id != rideTime!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                rideTime.Id = id;
                rideTime.DriverId = vm.DriverId;
                rideTime.ScheduleId = vm.ScheduleId;
                rideTime.RideDateTime = DateTime.Parse(vm.RideTime).ToUniversalTime();
                rideTime.IsTaken = vm.IsTaken;
                rideTime.UpdatedBy = User.Identity!.Name!;
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

    // GET: AdminArea/RideTimes/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, null, roleName);
        if (rideTime == null) return NotFound();

        vm.Driver = rideTime.Driver!.AppUser!.LastAndFirstName;

        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt.ToString("G");
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt.ToString("G");
        vm.CreatedBy = rideTime.CreatedBy!;


        return View(vm);
    }

    // POST: AdminArea/RideTimes/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id, null, roleName);
        if (rideTime != null) _uow.RideTimes.Remove(rideTime);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RideTimeExists(Guid id)
    {
        return _uow.RideTimes.Exists(id);
    }
}