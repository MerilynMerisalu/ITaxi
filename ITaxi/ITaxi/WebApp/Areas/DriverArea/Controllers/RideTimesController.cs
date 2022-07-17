using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.DriverArea.Controllers
{
    [Area("DriverArea")]
    public class RideTimesController : Controller
    {
        private readonly AppDbContext _context;

        public RideTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DriverArea/RideTimes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RideTimes.Include(r => r.Driver).Include(r => r.Schedule);
            return View(await appDbContext.ToListAsync());
        }

        // GET: DriverArea/RideTimes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.RideTimes == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes
                .Include(r => r.Driver)
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            return View(rideTime);
        }

        // GET: DriverArea/RideTimes/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id");
            return View();
        }

        // POST: DriverArea/RideTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,ScheduleId,RideDateTime,IsTaken,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RideTime rideTime)
        {
            if (ModelState.IsValid)
            {
                rideTime.Id = Guid.NewGuid();
                _context.Add(rideTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", rideTime.DriverId);
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // GET: DriverArea/RideTimes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RideTimes == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes.FindAsync(id);
            if (rideTime == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", rideTime.DriverId);
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // POST: DriverArea/RideTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,ScheduleId,RideDateTime,IsTaken,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RideTime rideTime)
        {
            if (id != rideTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", rideTime.DriverId);
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // GET: DriverArea/RideTimes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.RideTimes == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes
                .Include(r => r.Driver)
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            return View(rideTime);
        }

        // POST: DriverArea/RideTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.RideTimes == null)
            {
                return Problem("Entity set 'AppDbContext.RideTimes'  is null.");
            }
            var rideTime = await _context.RideTimes.FindAsync(id);
            if (rideTime != null)
            {
                _context.RideTimes.Remove(rideTime);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RideTimeExists(Guid id)
        {
          return (_context.RideTimes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
