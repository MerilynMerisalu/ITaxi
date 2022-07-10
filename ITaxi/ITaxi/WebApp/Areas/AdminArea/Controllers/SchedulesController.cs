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
            return View(await _uow.Schedules.GettingAllOrderedSchedulesWithIncludesAsync());
        }

        // GET: AdminArea/Schedules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteScheduleViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _uow.Schedules.FirstOrDefaultAsync(id.Value);
                
            if (schedule == null)
            {
                return NotFound();
            }

            vm.Id = schedule.Id;
            vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
            vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
            vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
            vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");

            return View(vm);
        }

        // GET: AdminArea/Schedules/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateScheduleViewModel();
            vm.Vehicles = new SelectList( await _uow.Vehicles.GettingOrderedVehiclesAsync(),
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
                var driver = await _uow.Drivers.FirstAsync();
                schedule.Id = Guid.NewGuid();
                if (driver != null) schedule.DriverId = driver.Id;
                schedule.StartDateAndTime = DateTime.Parse(vm.StartDateAndTime).ToUniversalTime();
                schedule.EndDateAndTime = DateTime.Parse(vm.EndDateAndTime).ToUniversalTime();
                _uow.Schedules.Add(schedule);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new EditScheduleViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _uow.Schedules.FirstOrDefaultAsync(id.Value);
            if (schedule == null)
            {
                return NotFound();
            }

            vm.VehicleId = schedule.VehicleId;
            vm.StartDateAndTime = schedule.StartDateAndTime;
            vm.EndDateAndTime = schedule.EndDateAndTime;
            vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(),
                nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier)); 
           
            return View(vm);
        }

        // POST: AdminArea/Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditScheduleViewModel vm )
        {
            var schedule = await _uow.Schedules.FirstOrDefaultAsync(id);
            
            if (schedule != null && id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (schedule != null)
                    {
                        schedule.Id = id;
                        if (schedule.Driver != null)
                        {
                            schedule.Driver = await _uow.Drivers.FirstAsync();
                            schedule.DriverId = schedule.DriverId;
                            schedule.StartDateAndTime = vm.StartDateAndTime;
                            schedule.EndDateAndTime = vm.EndDateAndTime;
                        }
                        
                        _uow.Schedules.Update(schedule);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (schedule != null && !ScheduleExists(schedule.Id))
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

        // GET: AdminArea/Schedules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteScheduleViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _uow.Schedules.FirstOrDefaultAsync(id.Value);
            if (schedule == null)
            {
                return NotFound();
            }

            vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
            vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
            vm.StartDateAndTime = schedule.StartDateAndTime.ToString("g");
            vm.EndDateAndTime = schedule.EndDateAndTime.ToString("g");

            return View(vm);
        }

        // POST: AdminArea/Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var schedule = await _uow.Schedules.FirstOrDefaultAsync(id);
            if (await _uow.RideTimes.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id))
                || await _uow.Bookings.AnyAsync(s => s!.ScheduleId.Equals(schedule!.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (schedule != null) _uow.Schedules.Remove(schedule);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(Guid id)
        {
            return _uow.Schedules.Exists(id);
        }
    }
}
