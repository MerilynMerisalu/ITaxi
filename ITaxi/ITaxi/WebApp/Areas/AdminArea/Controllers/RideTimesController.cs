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
    #warning Ask if this is the right way to get the user name of a logged in user
#warning Ask how to get the user role using interface


    public RideTimesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/RideTimes
    public async Task<IActionResult> Index()
    {
        
        var res = await _uow.RideTimes.GettingAllOrderedRideTimesAsync(null, null);
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
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, null, null);
        
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
        vm.Schedules = new SelectList(new Schedule[0]
            , nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        vm.RideTimes = new SelectList(new string[0]);
    
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

    public class SetDropDownListRequest
    {
        public string ListType { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// Generic method that will update the VM to reflect the new SelectLists if any need to be changed
    /// </summary>
    /// <param name="listType">the dropdownlist that has been changed</param>
    /// <param name="value">The currently selected item value</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetDropDownList([FromBody]SetDropDownListRequest parameters)
    {
        // Use the EditRideTimeViewModel because we want to send through the SelectLists and Ids that have now changed
        var vm = new EditRideTimeViewModel();
        IEnumerable<Schedule> schedules = null;
        //Guid id = Guid.Parse(value);

        if (parameters.ListType == nameof(RideTime.DriverId))
        {
            // refresh the list of schedules for the selected driver
            // the value, represents the current DriverId
            var driverId = Guid.Parse(parameters.Value);

            schedules = await _uow.Schedules.GettingTheScheduleByDriverIdAsync(driverId, null);
            foreach (var schedule in schedules)
            {
                schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
                schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
            }

            vm.Schedules = new SelectList(
                schedules,
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));

            // For now select the first schedule, later you might want to pick the schedule that is closest or overlaps with the previous selection
            vm.ScheduleId = schedules.FirstOrDefault().Id;
        }
        else if (parameters.ListType == nameof(RideTime.ScheduleId))
        {
            vm.ScheduleId = Guid.Parse(parameters.Value);
            
            // reload the schedules, but just the current one so we can rebuild the ride times
            schedules = new[] { await _uow.Schedules.GettingTheFirstScheduleByIdAsync(vm.ScheduleId, null) }!;
            foreach (var schedule in schedules)
            {
                schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
                schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
            }
            
            // NOTE: we do not need to rebuild the SelectList for schedules, only the RideTimes
        }

        // Always refresh the RideTimes, if the DriverId or the ScheduleId are changed
        // => because the ScheduleId is ALWAYS changed when the DriverId is changed.
        // Select the RideTimes form the currently selected schedule, for the current driver
        var currentSchedule = schedules.Where(x => x.Id == vm.ScheduleId).ToArray();
        var rideTimes = _uow.RideTimes.CalculatingRideTimes(_uow.Schedules.GettingStartAndEndTime(currentSchedule));
        // the times in schedules have already been converted!
        vm.RideTimes = new SelectList(rideTimes.Select(x => new { RideTime = x }), 
            nameof(vm.RideTime), nameof(vm.RideTime));

        // we need to select one of these!
        #warning: like with the selection of the ScheduleId when the driver is change, you might want to select a specific ride time, not just the first one
        vm.RideTime = rideTimes.First();
        
        return Ok(vm);
    }

    // GET: AdminArea/RideTimes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditRideTimeViewModel();
        if (id == null) return NotFound();

        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, null, null);
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        vm.DriverId = rideTime.DriverId;
        vm.Drivers = new SelectList(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), "AppUser.LastAndFirstName");

        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        
        var schedules = await _uow.Schedules.GettingTheScheduleByDriverIdAsync(rideTime.DriverId, null, null);
        foreach (var schedule in schedules)
        {
            schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
            schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
        }

        vm.Schedules = new SelectList(
            schedules,
            nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
        
        vm.IsTaken = rideTime.IsTaken;
#warning Ridetimes should be hidden and reappearing based on whether IsTaken is true or not
        // Select the RideTimes form the currently selected schedule, for the current driver
        var currentSchedule = schedules.Where(x => x.Id == rideTime.ScheduleId).ToArray();
        var rideTimes = _uow.RideTimes.CalculatingRideTimes(_uow.Schedules.GettingStartAndEndTime(currentSchedule));
#warning Ask if there is a better way to implement this
        var rideTimeList = new List<string>();
        // the times in schedules have already been converted!
        vm.RideTimes = new SelectList(rideTimes.Select(x => new { RideTime = x }), nameof(vm.RideTime), nameof(vm.RideTime));
        vm.ScheduleId = rideTime.ScheduleId;
       
        return View(vm);
    }

    // POST: AdminArea/RideTimes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid? id, EditRideTimeViewModel vm)
    {
        
        var rideTime = await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id!.Value, null, null);
        
        if (rideTime == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                rideTime.Id = id.Value;
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
        
        var rideTime =  await _uow.RideTimes.GettingFirstRideTimeByIdAsync(id.Value, null, null);
        if (rideTime == null) return NotFound();

        

        rideTime.Schedule!.StartDateAndTime = rideTime.Schedule.StartDateAndTime.ToLocalTime();
        rideTime.Schedule!.EndDateAndTime = rideTime.Schedule.EndDateAndTime.ToLocalTime();

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