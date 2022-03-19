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

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DrivesController : Controller
    {
        private readonly AppDbContext _context;

        public DrivesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Drives
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Drives
                .Include(d => d.Booking)
                .ThenInclude(d => d.Schedule)
                .Include(c => c.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(c => c.Booking)
                .ThenInclude(c => c.City)
                .Include(b => b.Booking)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Drives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drive == null)
            {
                return NotFound();
            }

            return View(drive);
        }

        /*
        // GET: AdminArea/Drives/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
            return View();
        }

        // POST: AdminArea/Drives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
        {
            if (ModelState.IsValid)
            {
                drive.Id = Guid.NewGuid();
                _context.Add(drive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives.FindAsync(id);
            if (drive == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // POST: AdminArea/Drives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
        {
            if (id != drive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriveExists(drive.Id))
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drive == null)
            {
                return NotFound();
            }

            return View(drive);
        }

        // POST: AdminArea/Drives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var drive = await _context.Drives.FindAsync(id);
            _context.Drives.Remove(drive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        private bool DriveExists(Guid id)
        {
            return _context.Drives.Any(e => e.Id == id);
        }

        /// <summary>
        /// Search drives by inserted date 
        /// </summary>
        /// <param name="search">date</param>
        /// <returns>An index view with search results</returns>
        [HttpPost]
        public async Task<IActionResult> SearchByDateAsync([FromForm] DateTime search)
        {

            var drives = await _context.Drives.Include(b => b.Booking)
                .ThenInclude(b => b.Driver)
                .ThenInclude(d => d.AppUser)
                .Include(b => b.Booking)
                .ThenInclude(d => d.Schedule)
                .Include(b => b.Booking)
                .ThenInclude(v => v.Vehicle)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(v => v.Booking)
                .ThenInclude(v => v.VehicleType)
                .Include(d => d.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(c => c.Booking)
                .ThenInclude(c => c.City)
                .Include(c => c.Comment)
                .Where(d => d.Booking!.PickUpDateAndTime.Date.Equals(search.Date)).ToListAsync();
            return View(nameof(Index), drives);
        }
    }
}
