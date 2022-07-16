using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.Test.Controllers
{
    [Area("Test")]
    public class VehiclesController : Controller
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Test/Vehicles
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Vehicles.Include(v => v.Driver).Include(v => v.VehicleMark).Include(v => v.VehicleModel).Include(v => v.VehicleType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Test/Vehicles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Driver)
                .Include(v => v.VehicleMark)
                .Include(v => v.VehicleModel)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Test/Vehicles/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
            ViewData["VehicleMarkId"] = new SelectList(_context.VehicleMarks, "Id", "VehicleMarkName");
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "Id", "VehicleModelName");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Id");
            return View();
        }

        // POST: Test/Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,VehicleTypeId,VehicleMarkId,VehicleModelId,VehiclePlateNumber,ManufactureYear,NumberOfSeats,VehicleAvailability,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Id = Guid.NewGuid();
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", vehicle.DriverId);
            ViewData["VehicleMarkId"] = new SelectList(_context.VehicleMarks, "Id", "VehicleMarkName", vehicle.VehicleMarkId);
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "Id", "VehicleModelName", vehicle.VehicleModelId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Test/Vehicles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", vehicle.DriverId);
            ViewData["VehicleMarkId"] = new SelectList(_context.VehicleMarks, "Id", "VehicleMarkName", vehicle.VehicleMarkId);
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "Id", "VehicleModelName", vehicle.VehicleModelId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Test/Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,VehicleTypeId,VehicleMarkId,VehicleModelId,VehiclePlateNumber,ManufactureYear,NumberOfSeats,VehicleAvailability,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", vehicle.DriverId);
            ViewData["VehicleMarkId"] = new SelectList(_context.VehicleMarks, "Id", "VehicleMarkName", vehicle.VehicleMarkId);
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "Id", "VehicleModelName", vehicle.VehicleModelId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Test/Vehicles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Driver)
                .Include(v => v.VehicleMark)
                .Include(v => v.VehicleModel)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Test/Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'AppDbContext.Vehicles'  is null.");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(Guid id)
        {
          return (_context.Vehicles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
