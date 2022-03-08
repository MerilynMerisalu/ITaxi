#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class RideTimesController : Controller
    {
        private readonly AppDbContext _context;

        public RideTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/RideTimes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RideTimes.
                Include(r => r.Schedule);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/RideTimes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteRideTimeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes
                .Include(r => r.Schedule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.Id = rideTime.Id;
            vm.ShiftDurationTime = rideTime.Schedule!.ShiftDurationTime;
            vm.RideTime = rideTime.RideDateTime.ToString("t");
            vm.IsTaken = rideTime.IsTaken;

            return View(vm);
        }

        // GET: AdminArea/RideTimes/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateRideTimeViewModel();
            vm.Schedules = new SelectList(await _context.Schedules.OrderBy(s => s.StartDateAndTime)
                    .Select(s => s).ToListAsync()
                , nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
            var scheduleStartDateAndTime = await GettingScheduleStartDateAndTimeAsync();
            var scheduleEndDateAndTime = await GettingScheduleEndDateAndTimeAsync();
            var rideTimes = GettingRideTimes( scheduleStartDateAndTime, scheduleEndDateAndTime);
            vm.RideTimes = GettingRideTimeSelectList(rideTimes);
            return View(vm);
        }

        // POST: AdminArea/RideTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRideTimeViewModel vm, ICollection<RideTime> rideTimes)
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
                            RideDateTime = selectedRideTime,
                            IsTaken = vm.IsTaken
                        };
                        
                         rideTimes.Add(rideTime);
                    }

                    await _context.RideTimes.AddRangeAsync(rideTimes);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            
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

            var rideTime = await _context.RideTimes.SingleOrDefaultAsync( r => r.Id.Equals(id));
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.Id = rideTime.Id;
            vm.Schedules = new SelectList(
                await _context.Schedules.Select(s => new {s.Id, s.ShiftDurationTime}).ToListAsync(),
                nameof(Schedule.Id), nameof(Schedule.ShiftDurationTime));
            vm.IsTaken = rideTime.IsTaken;
            #warning Ridetimes should be hidden and reappearing based on whether IsTaken is true or not
            vm.RideTimes = new SelectList(await _context.RideTimes
                .Where(r => r.ScheduleId.Equals(rideTime.ScheduleId))
                .Select(r => r.RideDateTime.ToString("t"))
                .ToListAsync());
            vm.ScheduleId = rideTime.ScheduleId;
            vm.RideTime = rideTime.RideDateTime.ToString("t");
            return View(vm);
        }

        // POST: AdminArea/RideTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditRideTimeViewModel vm)
        {
            var rideTime = await _context.RideTimes
                .SingleOrDefaultAsync(r => r.Id.Equals(id));
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
                    _context.Update(rideTime);
                    await _context.SaveChangesAsync();
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

            var rideTime = await _context.RideTimes
                .Include(r => r.Schedule)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            vm.ShiftDurationTime = rideTime.Schedule!.ShiftDurationTime;
            vm.RideTime = rideTime.RideDateTime.ToString("t");
            vm.IsTaken = rideTime.IsTaken;

            return View(vm);
        }

        // POST: AdminArea/RideTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rideTime = await _context.RideTimes.SingleOrDefaultAsync(r => r.Id.Equals(id));
            if (rideTime != null) _context.RideTimes.Remove(rideTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RideTimeExists(Guid id)
        {
            return _context.RideTimes.Any(e => e.Id == id);
        }
        
#warning GettingScheduleStartDateAndTimeAsync needs fixing
        
        /// <summary>
        /// This will return the schedule start date and time
        /// </summary>
        /// <returns>Datetime start</returns>
        private async Task<DateTime> GettingScheduleStartDateAndTimeAsync()
        {
            var start = await _context.Schedules
                .Select(s => s.StartDateAndTime)
                .OrderBy(s => s.Hour)
                .ThenBy(s => s.Minute).FirstOrDefaultAsync();

            return start;
        }

        #warning GettingScheduleEndDateAndTimeAsync needs fixing
        /// <summary>
        /// This will return the schedule end date and time
        /// </summary>
        /// <returns>Datetime end</returns>
        private async Task<DateTime> GettingScheduleEndDateAndTimeAsync()
        {
            var end = await _context.Schedules
                .Select(s => s.EndDateAndTime).OrderBy(s => s.Hour)
                .ThenBy(s => s.Minute).LastOrDefaultAsync();
            return end;
        }
        /// <summary>
        /// Getting a list of times as strings
        /// </summary>
        /// <param name="start">The start time of a schedule</param>
        /// <param name="end">The end time of a schedule</param>
        /// <returns>list of times as strings</returns>
        private List<string> GettingRideTimes(DateTime start, DateTime end)
        {
            List<string> times = new List<string>();
            var time = start;

            while (time < end)
            {
                times.Add(time.ToString("t"));
                time = time.AddMinutes(45);
            }

            return times;
        }
        /// <summary>
        /// Returning times as a select list
        /// </summary>
        /// <param name="times">List of drive times for a driver</param>
        /// <returns>Selectlist rideTimes</returns>
        private SelectList GettingRideTimeSelectList(List<string> times)
        {
            var rideTimes = new SelectList(times);
            return rideTimes;
        }
    }
}
