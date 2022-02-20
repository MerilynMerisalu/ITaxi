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
    public class VehicleMarksController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleMarksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VehicleMarks
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleMarks.ToListAsync());
        }

        // GET: VehicleMarks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            return View(vehicleMark);
        }

        // GET: VehicleMarks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleMarkName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VehicleMark vehicleMark)
        {
            if (ModelState.IsValid)
            {
                vehicleMark.Id = Guid.NewGuid();
                _context.Add(vehicleMark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMark);
        }

        // GET: VehicleMarks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks.FindAsync(id);
            if (vehicleMark == null)
            {
                return NotFound();
            }
            return View(vehicleMark);
        }

        // POST: VehicleMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VehicleMarkName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] VehicleMark vehicleMark)
        {
            if (id != vehicleMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleMark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMarkExists(vehicleMark.Id))
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
            return View(vehicleMark);
        }

        // GET: VehicleMarks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            return View(vehicleMark);
        }

        // POST: VehicleMarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleMark = await _context.VehicleMarks.FindAsync(id);
            _context.VehicleMarks.Remove(vehicleMark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMarkExists(Guid id)
        {
            return _context.VehicleMarks.Any(e => e.Id == id);
        }
    }
}
