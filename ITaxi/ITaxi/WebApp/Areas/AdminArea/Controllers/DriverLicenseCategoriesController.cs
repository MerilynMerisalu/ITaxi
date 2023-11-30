#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area driver license categories controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class DriverLicenseCategoriesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area driver license categories controller constructor
    /// </summary>
    /// <param name="appBLL">AppBll</param>
    public DriverLicenseCategoriesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/DriverLicenseCategories
    /// <summary>
    /// Admin area driver license categories index
    /// </summary>
    /// <returns>View with data</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync();
        
        return View(res);
    }

    // GET: AdminArea/DriverLicenseCategories/Details/5
    /// <summary>
    /// Admin area driver license categories details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
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
    /// <summary>
    /// Admin area driver license categories create GET method
    /// </summary>
    /// <returns>View model</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditDriverLicenseCategoryViewModel();
        return View(vm);
    }

    // POST: AdminArea/DriverLicenseCategories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area driver license categories create POST method
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="driverLicenseCategory">Driver license category</param>
    /// <returns>View model</returns>
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
    /// <summary>
    /// Admin area driver license categories edit GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
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
    /// <summary>
    /// Admin area driver license categories edit POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View model</returns>
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
    /// <summary>
    /// Admin area driver license categories delete GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
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
    /// <summary>
    /// Admin area driver license categories delete POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect user to index page</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);

        if (await _appBLL.DriverAndDriverLicenseCategories.HasAnyDriversAsync(id))
        {
            return Content("Entity cannot be deleted because it has dependent entities!");
        }
        
        if (driverLicenseCategory != null) await _appBLL.DriverLicenseCategories.RemoveAsync(driverLicenseCategory.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DriverLicenseCategoryExists(Guid id)
    {
        return _appBLL.DriverLicenseCategories.Exists(id);
    }
}