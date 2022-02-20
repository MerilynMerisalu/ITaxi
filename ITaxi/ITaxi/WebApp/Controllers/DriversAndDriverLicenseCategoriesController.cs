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
    public class DriversAndDriverLicenseCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public DriversAndDriverLicenseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DriversAndDriverLicenseCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DriverAndDriverLicenseCategories.Include(d => d.Driver).Include(d => d.DriverLicenseCategory);
            return View(await appDbContext.ToListAsync());
        }

        // GET: DriversAndDriverLicenseCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories
                .Include(d => d.Driver)
                .Include(d => d.DriverLicenseCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverAndDriverLicenseCategory == null)
            {
                return NotFound();
            }

            return View(driverAndDriverLicenseCategory);
        }

        // GET: DriversAndDriverLicenseCategories/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
            ViewData["DriverLicenseCategoryId"] = new SelectList(_context.DriverLicenseCategories, "Id", "DriverLicenseCategoryName");
            return View();
        }

        // POST: DriversAndDriverLicenseCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,DriverLicenseCategoryId,Id")] DriverAndDriverLicenseCategory driverAndDriverLicenseCategory)
        {
            if (ModelState.IsValid)
            {
                driverAndDriverLicenseCategory.Id = Guid.NewGuid();
                _context.Add(driverAndDriverLicenseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", driverAndDriverLicenseCategory.DriverId);
            ViewData["DriverLicenseCategoryId"] = new SelectList(_context.DriverLicenseCategories, "Id", "DriverLicenseCategoryName", driverAndDriverLicenseCategory.DriverLicenseCategoryId);
            return View(driverAndDriverLicenseCategory);
        }

        // GET: DriversAndDriverLicenseCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories.FindAsync(id);
            if (driverAndDriverLicenseCategory == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", driverAndDriverLicenseCategory.DriverId);
            ViewData["DriverLicenseCategoryId"] = new SelectList(_context.DriverLicenseCategories, "Id", "DriverLicenseCategoryName", driverAndDriverLicenseCategory.DriverLicenseCategoryId);
            return View(driverAndDriverLicenseCategory);
        }

        // POST: DriversAndDriverLicenseCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,DriverLicenseCategoryId,Id")] DriverAndDriverLicenseCategory driverAndDriverLicenseCategory)
        {
            if (id != driverAndDriverLicenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverAndDriverLicenseCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverAndDriverLicenseCategoryExists(driverAndDriverLicenseCategory.Id))
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", driverAndDriverLicenseCategory.DriverId);
            ViewData["DriverLicenseCategoryId"] = new SelectList(_context.DriverLicenseCategories, "Id", "DriverLicenseCategoryName", driverAndDriverLicenseCategory.DriverLicenseCategoryId);
            return View(driverAndDriverLicenseCategory);
        }

        // GET: DriversAndDriverLicenseCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories
                .Include(d => d.Driver)
                .Include(d => d.DriverLicenseCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverAndDriverLicenseCategory == null)
            {
                return NotFound();
            }

            return View(driverAndDriverLicenseCategory);
        }

        // POST: DriversAndDriverLicenseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var driverAndDriverLicenseCategory = await _context.DriverAndDriverLicenseCategories.FindAsync(id);
            _context.DriverAndDriverLicenseCategories.Remove(driverAndDriverLicenseCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverAndDriverLicenseCategoryExists(Guid id)
        {
            return _context.DriverAndDriverLicenseCategories.Any(e => e.Id == id);
        }
    }
}
