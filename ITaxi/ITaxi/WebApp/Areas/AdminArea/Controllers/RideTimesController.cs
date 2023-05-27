#nullable enable

using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using DriverDTO = App.BLL.DTO.AdminArea.DriverDTO;
using ScheduleDTO = App.BLL.DTO.AdminArea.ScheduleDTO;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class RideTimesController : Controller
{
    private readonly IAppBLL _appBLL;
#warning Ask if this is the right way to get the user name of a logged in user
#warning Ask how to get the user role using interface


    public RideTimesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/RideTimes
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.RideTimes.GettingAllOrderedRideTimesAsync(null, null);
        
        return View(res);
    }

    // GET: AdminArea/RideTimes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteRideTimeViewModel();
        if (id == null) return NotFound();
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value);
        
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        vm.Driver = rideTime.Schedule!.Driver!.AppUser!.LastAndFirstName;
        vm.Schedule = rideTime.Schedule!.ShiftDurationTime;
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;

        return View(vm);
    }

    // GET: AdminArea/RideTimes/Create
    public async Task<IActionResult> Create()
    {
        var roleName = User.GettingUserRoleName();
        var vm = new CreateRideTimeViewModel();
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");
        vm.Schedules = new SelectList(new Schedule[0]
            , nameof(ScheduleDTO.Id), nameof(Schedule.ShiftDurationTime));
        vm.RideTimes = new SelectList(new string[0]);
    
        return View(vm);
    }

    // POST: AdminArea/RideTimes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                 _appBLL.RideTimes.AddRange(rideTimes);
                await _appBLL.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            // custom validation to check that at least one ride time is chosen
            // This logic has been replicated in RequiredAtLeastOneSelectionAttribute
            // But we leave this code here just in case the attribute is not in place.
            else
            {
                #warning replace this string literal with a language resource
                ModelState.AddModelError(nameof(vm.SelectedRideTimes), "Please select at least 1 time");
            }
        }

        // After a Model Error, the VM is reset, so we need to rebuild the 
        // lists so that the user can continue to complete the form
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(Driver.Id), "AppUser.LastAndFirstName");

        if (vm.DriverId != Guid.Empty)
        {
            // we can only set the schedules after the driver has been selected.
            var schedules = await _appBLL.Schedules.GettingTheScheduleByDriverIdAsync(vm.DriverId, null, null);
            foreach (var schedule in schedules)
            {
                schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
                schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
            }

            vm.Schedules = new SelectList(
                schedules,
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));

            if (vm.ScheduleId != Guid.Empty)
            {
                // Select the RideTimes form the currently selected schedule, for the current driver
                var rideTimesList = _appBLL.RideTimes.GettingRemainingRideTimesByScheduleId(vm.ScheduleId);
                
                // the times in schedules have already been converted!
                vm.RideTimes = new SelectList(rideTimesList.Select(x => new {RideTime = x}), "RideTime", "RideTime");
            }
        }

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
    public async Task<IActionResult> SetDropDownList([FromBody] SetDropDownListRequest parameters)
    {
        // Use the EditRideTimeViewModel because we want to send through the SelectLists and Ids that have now changed
        var vm = new EditRideTimeViewModel();
        IEnumerable<ScheduleDTO> schedules = null;
        //Guid id = Guid.Parse(value);

        if (parameters.ListType == nameof(RideTime.DriverId))
        {
            // refresh the list of schedules for the selected driver
            // the value, represents the current DriverId
            var driverId = Guid.Parse(parameters.Value);

            schedules = await _appBLL.Schedules.GettingTheScheduleByDriverIdAsync(driverId, null);
 
            //_appBLL.Schedules.
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
            schedules = new[] {await _appBLL.Schedules.GettingTheFirstScheduleByIdAsync(vm.ScheduleId, null)}!;

            // NOTE: we do not need to rebuild the SelectList for schedules, only the RideTimes
        }

        // Always refresh the RideTimes, if the DriverId or the ScheduleId are changed
        // => because the ScheduleId is ALWAYS changed when the DriverId is changed.
        // Select the RideTimes form the currently selected schedule, for the current driver
        var rideTimes = _appBLL.RideTimes.GettingRemainingRideTimesByScheduleId(vm.ScheduleId);

        // the times in schedules have already been converted!
        vm.RideTimes = new SelectList(rideTimes.Select(x => new {RideTime = x}),
            nameof(vm.RideTime), nameof(vm.RideTime));

        // we need to select one of these!
#warning: like with the selection of the ScheduleId when the driver is change, you might want to select a specific ride time, not just the first one
        vm.RideTime = rideTimes.First();


        return Ok(vm);
    }

    // GET: AdminArea/RideTimes/Edit/5
    /*public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditRideTimeViewModel();
        if (id == null) return NotFound();

        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id.Value);
        if (rideTime == null) return NotFound();

        vm.Id = rideTime.Id;
        vm.DriverId = rideTime.DriverId;
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");

        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        
        var schedules = await _appBLL.Schedules.GettingTheScheduleByDriverIdAsync(rideTime.DriverId, null, null);
        foreach (var schedule in schedules)
        {
            schedule.StartDateAndTime = schedule.StartDateAndTime.ToLocalTime();
            schedule.EndDateAndTime = schedule.EndDateAndTime.ToLocalTime();
        }

        vm.Schedules = new SelectList(
            schedules,
            nameof(ScheduleDTO.Id), nameof(ScheduleDTO.ShiftDurationTime));
        
        vm.IsTaken = rideTime.IsTaken;
#warning Ridetimes should be hidden and reappearing based on whether IsTaken is true or not
        // Select the RideTimes form the currently selected schedule, for the current driver
        var rideTimes = _appBLL.RideTimes.CalculatingRideTimes(rideTime.ScheduleId);
        vm.RideTimes = new SelectList(rideTimes.Select(x => new { RideTime = x }), 
            nameof(vm.RideTime), nameof(vm.RideTime));
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
        
        var rideTime = await _appBLL.RideTimes.GettingFirstRideTimeByIdAsync(id!.Value);
        
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
    }*/

    // GET: AdminArea/RideTimes/Delete/5
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
#warning Should it be a repository method
        vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("t");
        vm.IsTaken = rideTime.IsTaken;
        vm.CreatedAt = rideTime.CreatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;
        vm.UpdatedAt = rideTime.UpdatedAt;
        vm.CreatedBy = rideTime.CreatedBy!;


        return View(vm);
    }

    // POST: AdminArea/RideTimes/Delete/5
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