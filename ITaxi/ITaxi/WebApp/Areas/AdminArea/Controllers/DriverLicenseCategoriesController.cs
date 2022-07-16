#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    [Authorize(Roles = nameof(Admin))]
    public class DriverLicenseCategoriesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public DriverLicenseCategoriesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/DriverLicenseCategories
        public async Task<IActionResult> Index()
        {
            var res = await _uow.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync();
#warning Should this be a repo method
            foreach (var driverLicenseCategory in res)
            {
                driverLicenseCategory.CreatedAt = driverLicenseCategory.CreatedAt.ToLocalTime();
                driverLicenseCategory.UpdatedAt = driverLicenseCategory.UpdatedAt.ToLocalTime();
            }

            return View(res);
        }

        // GET: AdminArea/DriverLicenseCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new DetailsDeleteDriverLicenseCategoryViewModel();
            var driverLicenseCategory = await _uow.DriverLicenseCategories
                .FirstOrDefaultAsync(id.Value);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
            vm.CreatedBy = driverLicenseCategory.CreatedBy!;
            vm.CreatedAt = driverLicenseCategory.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = driverLicenseCategory.UpdatedBy!;
            vm.UpdatedAt = driverLicenseCategory.UpdatedAt.ToLocalTime().ToString("G");

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
                _uow.DriverLicenseCategories.Add(driverLicenseCategory);
                await _uow.SaveChangesAsync();
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

            var driverLicenseCategory = await _uow.DriverLicenseCategories
                .FirstOrDefaultAsync(id.Value);
            if (driverLicenseCategory != null)
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
            var driverLicenseCategory = await _uow.DriverLicenseCategories.FirstOrDefaultAsync(id);
            if (driverLicenseCategory != null && id != driverLicenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (driverLicenseCategory != null)
                    {
                        driverLicenseCategory.Id = id;
                        driverLicenseCategory.DriverLicenseCategoryName = vm.DriverLicenseCategoryName;
                        driverLicenseCategory.UpdatedAt = DateTime.UtcNow;
                        _uow.DriverLicenseCategories.Update(driverLicenseCategory);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (driverLicenseCategory != null && !DriverLicenseCategoryExists(driverLicenseCategory.Id))
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

            var driverLicenseCategory = await _uow.DriverLicenseCategories.FirstOrDefaultAsync(id.Value);
            if (driverLicenseCategory == null)
            {
                return NotFound();
            }

            vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
            vm.CreatedBy = driverLicenseCategory.CreatedBy!;
            vm.CreatedAt = driverLicenseCategory.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = driverLicenseCategory.UpdatedBy!;
            vm.UpdatedAt = driverLicenseCategory.UpdatedAt.ToLocalTime().ToString("G");
            return View(vm);
        }

        // POST: AdminArea/DriverLicenseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var driverLicenseCategory = await _uow.DriverLicenseCategories.FirstOrDefaultAsync(id);
            #warning Ask if this could be improved
            if ( await _uow.DriverAndDriverLicenseCategories
                    .AnyAsync(c => c != null && c.DriverLicenseCategoryId.Equals(id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (driverLicenseCategory != null) _uow.DriverLicenseCategories.Remove(driverLicenseCategory);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverLicenseCategoryExists(Guid id)
        {
            return _uow.DriverLicenseCategories.Exists(id);
        }
    }
}
