#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class RideTimesController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        

        public RideTimesController( IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/RideTimes
        public async Task<IActionResult> Index()
        {
            var res = await _uow.RideTimes.GettingAllOrderedRideTimesAsync();
            #warning Should this be a repository method
            foreach (var rideTime in res)
            {
                if (rideTime != null)
                {
                    rideTime.RideDateTime = rideTime.RideDateTime.ToLocalTime();
                    rideTime.CreatedAt = rideTime.CreatedAt.ToLocalTime();
                    rideTime.UpdatedAt = rideTime.UpdatedAt.ToLocalTime();
                }
                    
            }

            return View(res);
        }

        // GET: AdminArea/RideTimes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteRideTimeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id.Value);
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.Id = rideTime.Id;
            vm.ShiftDurationTime = rideTime.Schedule!.ShiftDurationTime;
            #warning Should it be a repository method
            vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("g");
            vm.IsTaken = rideTime.IsTaken;

            return View(vm);
        }

        // GET: AdminArea/RideTimes/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateRideTimeViewModel();
            vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync()
                , nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
            DateTime[] scheduleStartAndEndTime = _uow.Schedules.GettingStartAndEndTime();
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
                        var rideTime = new RideTime()
                        {
                            Id = new Guid(),
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
            vm.Schedules = new SelectList(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(), nameof(Schedule.Id),
                nameof(Schedule.ShiftDurationTime));
           #warning Selectable ride times must be recreated when something goes wrong with creating the record
            #warning Selected ride times remain so when something goes wrong with creating the record
            return View(vm);
        }

        // GET: AdminArea/RideTimes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new EditRideTimeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id.Value);
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.Id = rideTime.Id;
            vm.Schedules = new SelectList(
                 await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync(),
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
            vm.IsTaken = rideTime.IsTaken;
            #warning Ridetimes should be hidden and reappearing based on whether IsTaken is true or not
            vm.RideTimes = new SelectList(await _uow.RideTimes.GettingAllSelectedRideTimesAsync(rideTime),
                nameof(RideTime.Id), nameof(RideTime.RideDateTime));
            vm.ScheduleId = rideTime.ScheduleId;
            #warning Should it be a repository method
            vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("g");
            return View(vm);
        }

        // POST: AdminArea/RideTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditRideTimeViewModel vm)
        {
            var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id);
                
            if (id != rideTime!.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rideTime.Id = id;
                    rideTime.ScheduleId = vm.ScheduleId;
                    rideTime.RideDateTime = DateTime.Parse(vm.RideTime);
                    rideTime.IsTaken = vm.IsTaken;
                    _uow.RideTimes.Update(rideTime);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RideTimeExists(rideTime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(vm);
        }

        // GET: AdminArea/RideTimes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteRideTimeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _uow.RideTimes.FirstOrDefaultAsync(id.Value);
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.ShiftDurationTime = rideTime.Schedule!.ShiftDurationTime;
            #warning Should it be a repository method
            vm.RideTime = rideTime.RideDateTime.ToLocalTime().ToString("g");
            vm.IsTaken = rideTime.IsTaken;
            

            return View(vm);
        }

        // POST: AdminArea/RideTimes/Delete/5
        [HttpPost, ActionName("Delete")]
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
}
