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
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DriverLicenseCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public DriverLicenseCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/DriverLicenseCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DriverLicenseCategories.ToListAsync());
        }

        // GET: AdminArea/DriverLicenseCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new DetailsDeleteDriverLicenseCategoryViewModel();
            var driverLicenseCategory = await _context.DriverLicenseCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;

            return View(vm);
        }

        // GET: AdminArea/DriverLicenseCategories/Create
        public IActionResult Create()
        {
            var vm = new CreateEditDriverLicenseCategoryViewModel();
            return View(vm);
        }

        // POST: AdminArea/DriverLicenseCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditDriverLicenseCategoryViewModel vm, 
            DriverLicenseCategory driverLicenseCategory)
        {
            if (ModelState.IsValid)
            {
                driverLicenseCategory.Id = Guid.NewGuid();
                driverLicenseCategory.DriverLicenseCategoryName = vm.DriverLicenseCategoryName;
                _context.Add(driverLicenseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/DriverLicenseCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditDriverLicenseCategoryViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var driverLicenseCategory = await _context.DriverLicenseCategories
                .SingleAsync(d => d.Id.Equals(id));
            vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
            return View(vm);
        }

        // POST: AdminArea/DriverLicenseCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditDriverLicenseCategoryViewModel vm)
        {
            var driverLicenseCategory = await _context.DriverLicenseCategories
                .OrderBy(dl => dl.DriverLicenseCategoryName)
                .SingleAsync();
            if (id != driverLicenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    driverLicenseCategory.Id = id;
                    driverLicenseCategory.DriverLicenseCategoryName = vm.DriverLicenseCategoryName;
                    driverLicenseCategory.UpdatedAt = DateTime.UtcNow;
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
            return View(vm);
        }

        // GET: AdminArea/DriverLicenseCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteDriverLicenseCategoryViewModel();
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

            vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
            return View(vm);
        }

        // POST: AdminArea/DriverLicenseCategories/Delete/5
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
