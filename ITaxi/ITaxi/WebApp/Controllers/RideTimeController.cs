#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Controllers
{
    public class RideTimeController : Controller
    {
        private readonly AppDbContext _context;

        public RideTimeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RideTime
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RideTimes.Include(r => r.Schedule);
            return View(await appDbContext.ToListAsync());
        }

        // GET: RideTime/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            return View(rideTime);
        }

        // GET: RideTime/Create
        public IActionResult Create()
        {
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id");
            return View();
        }

        // POST: RideTime/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,RideDateTime,IsTaken,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RideTime rideTime)
        {
            if (ModelState.IsValid)
            {
                rideTime.Id = Guid.NewGuid();
                _context.Add(rideTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // GET: RideTime/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes.FindAsync(id);
            if (rideTime == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // POST: RideTime/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ScheduleId,RideDateTime,IsTaken,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RideTime rideTime)
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
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", rideTime.ScheduleId);
            return View(rideTime);
        }

        // GET: RideTime/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rideTime = await _context.RideTimes
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rideTime == null)
            {
                return NotFound();
            }

            return View(rideTime);
        }

        // POST: RideTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rideTime = await _context.RideTimes.FindAsync(id);
            _context.RideTimes.Remove(rideTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RideTimeExists(Guid id)
        {
            return _context.RideTimes.Any(e => e.Id == id);
        }
    }
}
