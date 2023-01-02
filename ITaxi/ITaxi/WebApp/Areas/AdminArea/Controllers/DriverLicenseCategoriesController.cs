#nullable enable
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using DriverLicenseCategoryDTO = App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class DriverLicenseCategoriesController : Controller
{
    private readonly IAppBLL _appBLL;

    public DriverLicenseCategoriesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/DriverLicenseCategories
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync();


        return View(res);
    }

    // GET: AdminArea/DriverLicenseCategories/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new DetailsDeleteDriverLicenseCategoryViewModel();
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories
            .FirstOrDefaultAsync(id.Value);
        if (driverLicenseCategory == null) return NotFound();
        vm.Id = driverLicenseCategory.Id;
        vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
        vm.CreatedBy = driverLicenseCategory.CreatedBy!;
        vm.CreatedAt = driverLicenseCategory.CreatedAt;
        vm.UpdatedBy = driverLicenseCategory.UpdatedBy!;
        vm.UpdatedAt = driverLicenseCategory.UpdatedAt;

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
        DriverLicenseCategoryDTO driverLicenseCategory)
    {
        if (ModelState.IsValid)
        {
            driverLicenseCategory.Id = Guid.NewGuid();
            driverLicenseCategory.DriverLicenseCategoryName = vm.DriverLicenseCategoryName;
            driverLicenseCategory.CreatedBy = User.Identity!.Name;
            driverLicenseCategory.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.DriverLicenseCategories.Add(driverLicenseCategory);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/DriverLicenseCategories/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditDriverLicenseCategoryViewModel();
        if (id == null) return NotFound();

        var driverLicenseCategory = await _appBLL.DriverLicenseCategories
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
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);
        if (driverLicenseCategory != null && id != driverLicenseCategory.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (driverLicenseCategory != null)
                {
                    driverLicenseCategory.Id = id;
                    driverLicenseCategory.DriverLicenseCategoryName = vm.DriverLicenseCategoryName;
                    driverLicenseCategory.UpdatedAt = DateTime.Now.ToUniversalTime();
                    driverLicenseCategory.UpdatedBy = User.Identity!.Name;
                    _appBLL.DriverLicenseCategories.Update(driverLicenseCategory);
                }

                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (driverLicenseCategory != null && !DriverLicenseCategoryExists(driverLicenseCategory.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/DriverLicenseCategories/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteDriverLicenseCategoryViewModel();
        if (id == null) return NotFound();

        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id.Value);
        if (driverLicenseCategory == null) return NotFound();

        vm.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
        vm.CreatedBy = driverLicenseCategory.CreatedBy!;
        vm.CreatedAt = driverLicenseCategory.CreatedAt;
        vm.UpdatedBy = driverLicenseCategory.UpdatedBy!;
        vm.UpdatedAt = driverLicenseCategory.UpdatedAt;
        return View(vm);
    }

    // POST: AdminArea/DriverLicenseCategories/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);
#warning Ask if this could be improved
        /*if (await _appBLL.DriverAndDriverLicenseCategories
                .AnyAsync(c => c != null && c.DriverLicenseCategoryId.Equals(id)))
            return Content("Entity cannot be deleted because it has dependent entities!");*/
        if (driverLicenseCategory != null) _appBLL.DriverLicenseCategories.Remove(driverLicenseCategory);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DriverLicenseCategoryExists(Guid id)
    {
        return _appBLL.DriverLicenseCategories.Exists(id);
    }
}