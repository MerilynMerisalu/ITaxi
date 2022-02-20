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
    public class DriverLicenseCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public DriverLicenseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DriverLicenseCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DriverLicenseCategories.ToListAsync());
        }

        // GET: DriverLicenseCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLicenseCategory = await _context.DriverLicenseCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            return View(driverLicenseCategory);
        }

        // GET: DriverLicenseCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DriverLicenseCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverLicenseCategoryName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] DriverLicenseCategory driverLicenseCategory)
        {
            if (ModelState.IsValid)
            {
                driverLicenseCategory.Id = Guid.NewGuid();
                _context.Add(driverLicenseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driverLicenseCategory);
        }

        // GET: DriverLicenseCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLicenseCategory = await _context.DriverLicenseCategories.FindAsync(id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }
            return View(driverLicenseCategory);
        }

        // POST: DriverLicenseCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverLicenseCategoryName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] DriverLicenseCategory driverLicenseCategory)
        {
            if (id != driverLicenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverLicenseCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverLicenseCategoryExists(driverLicenseCategory.Id))
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
            return View(driverLicenseCategory);
        }

        // GET: DriverLicenseCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLicenseCategory = await _context.DriverLicenseCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            return View(driverLicenseCategory);
        }

        // POST: DriverLicenseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var driverLicenseCategory = await _context.DriverLicenseCategories.FindAsync(id);
            _context.DriverLicenseCategories.Remove(driverLicenseCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverLicenseCategoryExists(Guid id)
        {
            return _context.DriverLicenseCategories.Any(e => e.Id == id);
        }
    }
}
