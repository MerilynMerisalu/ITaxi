#nullable disable
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
            return View(await _uow.Schedules.GetAllAsync());
        }

        // GET: AdminArea/Schedules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteScheduleViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Driver)
                .ThenInclude(d=>d.AppUser)
                .Include(s => s.Vehicle)
                .ThenInclude(v=>v.VehicleMark)
                .Include(s=>s.Vehicle)
                .ThenInclude(v=>v.VehicleModel)
                .Include(s=>s.Vehicle)
                .ThenInclude(v=>v.VehicleType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            vm.Id = schedule.Id;
            vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
            vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
            vm.StartDateAndTime = schedule.StartDateAndTime;
            vm.EndDateAndTime = schedule.EndDateAndTime;

            return View(vm);
        }

        // GET: AdminArea/Schedules/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditScheduleViewModel();
            vm.Vehicles = new SelectList( await _context.Vehicles
                    .Include(v => v.VehicleMark)
                    .Include(v => v.VehicleModel)
                    .Include(v => v.VehicleType)
                    .OrderBy(c => c.VehicleMark.VehicleMarkName)
                .Select(c => new {c.Id, c.VehicleIdentifier}).ToListAsync(),
            nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier));
            #warning Schedule StartDateAndTime needs a custom validation
            vm.StartDateAndTime = Convert.ToDateTime(DateTime.Now.ToUniversalTime().ToString("g"));
            #warning Schedule EndDateAndTime needs a custom validation
            vm.EndDateAndTime = Convert.ToDateTime(DateTime.Now.AddHours(8).ToUniversalTime().ToString("g"));

            return View(vm);
        }

        // POST: AdminArea/Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditScheduleViewModel vm, Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                var driverId = await _context.Drivers.Select(d => d.Id).FirstOrDefaultAsync();
                schedule.Id = Guid.NewGuid();
                schedule.DriverId = driverId;
                schedule.StartDateAndTime = vm.StartDateAndTime;
                schedule.EndDateAndTime = vm.EndDateAndTime;
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditScheduleViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (schedule == null)
            {
                return NotFound();
            }

            vm.VehicleId = schedule.VehicleId;
            vm.StartDateAndTime = schedule.StartDateAndTime;
            vm.EndDateAndTime = schedule.EndDateAndTime;
            vm.Vehicles = new SelectList(await _context.Vehicles.Include(v =>
                        v.VehicleMark).Include(v => v.VehicleModel)
                    .Include(v => v.VehicleType).OrderBy(v => v.VehicleMark)
                    .Select(v => new {v.Id, v.VehicleIdentifier}).ToListAsync(),
                nameof(Vehicle.Id), nameof(Vehicle.VehicleIdentifier)); 
           
            return View(vm);
        }

        // POST: AdminArea/Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditScheduleViewModel vm )
        {
            var schedule = await _context.Schedules.SingleAsync(s => s.Id.Equals(id));
            
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    schedule.Id = id;
                    schedule.DriverId = _context.Schedules.SingleAsync(s => s.Id.Equals(id))
                        .Result.DriverId;
                    schedule.StartDateAndTime = vm.StartDateAndTime.ToUniversalTime();
                    schedule.EndDateAndTime = vm.EndDateAndTime.ToUniversalTime();
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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

            var schedule = await _context.Schedules
                .Include(s => s.Driver)
                .ThenInclude(d=> d.AppUser)
                .Include(s => s.Vehicle)
                .ThenInclude(v=>v.VehicleMark)
                .Include(s=>s.Vehicle)
                .ThenInclude(v=>v.VehicleModel)
                .Include(s=>s.Vehicle)
                .ThenInclude(v=>v.VehicleType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            vm.VehicleIdentifier = schedule.Vehicle!.VehicleIdentifier;
            vm.DriversFullName = schedule.Driver!.AppUser!.LastAndFirstName;
            vm.StartDateAndTime = schedule.StartDateAndTime;
            vm.EndDateAndTime = schedule.EndDateAndTime;

            return View(vm);
        }

        // POST: AdminArea/Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var schedule = await _context.Schedules.SingleOrDefaultAsync(s =>s.Id.Equals(id));
            if (await _context.RideTimes.AnyAsync(s => s.ScheduleId.Equals(schedule.Id))
                || await _context.Bookings.AnyAsync(s => s.ScheduleId.Equals(schedule.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (schedule != null) _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(Guid id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
