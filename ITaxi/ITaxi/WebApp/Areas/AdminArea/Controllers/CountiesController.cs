#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area counties controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class CountiesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area counties controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public CountiesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Counties
    /// <summary>
    /// Admin area counties controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Counties.GetAllCountiesOrderedByCountyNameAsync();
        
        return View(res);
    }

    // GET: AdminArea/Counties/Details/5
    /// <summary>
    /// Admin area counties controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCountyViewModel();
        if (id == null) return NotFound();

        var county = await _appBLL.Counties.FirstOrDefaultAsync(id.Value);
        if (county == null) return NotFound();

        vm.CountyName = county.CountyName;
        vm.CreatedAt = county.CreatedAt;
        vm.CreatedBy = county.CreatedBy!;
        vm.UpdatedAt = county.UpdatedAt;
        vm.UpdatedBy = county.UpdatedBy!;
        vm.Id = county.Id;

        return View(vm);
    }

    // GET: AdminArea/Counties/Create
    /// <summary>
    /// Admin area counties controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditCountyViewModel();
        return View(vm);
    }

    // POST: AdminArea/Counties/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area counties controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCountyViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var county = new CountyDTO();
            county.Id = Guid.NewGuid();
            county.CountyName = vm.CountyName;
            county.CreatedBy = User.GettingUserEmail();
            _appBLL.Counties.Add(county);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Counties/Edit/5
    /// <summary>
    /// Admin area counties controller GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCountyViewModel();
        if (id == null) return NotFound();

        var county = await _appBLL.Counties.FirstOrDefaultAsync(id.Value);

        if (county == null) return NotFound();

        vm.CountyName = county.CountyName;

        return View(vm);
    }

    // POST: AdminArea/Counties/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area counties controller POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditCountyViewModel vm)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id, true);
        if (ModelState.IsValid)
            if (county != null && county.Id.Equals(id))
            {
                try
                {
                    county.CountyName = vm.CountyName;
                    county.UpdatedBy = User.GettingUserEmail();
                    county.UpdatedAt = DateTime.Now;
                    _appBLL.Counties.Update(county);
                    await _appBLL.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountyExists(county.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

        return View(vm);
    }

    // GET: AdminArea/Counties/Delete/5
    /// <summary>
    /// Admin area counties controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCountyViewModel();
        if (id == null) return NotFound();

        var county = await _appBLL.Counties.FirstOrDefaultAsync(id.Value);
        if (county == null) return NotFound();

        vm.CountyName = county.CountyName;
        vm.CreatedAt = county.CreatedAt;
        vm.CreatedBy = county.CreatedBy ?? "";
        vm.UpdatedAt = county.UpdatedAt;
        vm.UpdatedBy = county.UpdatedBy ?? "";

        return View(vm);
    }

    // POST: AdminArea/Counties/Delete/5
    /// <summary>
    /// Admin area counties controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (county != null)
        {
            
            if (await _appBLL.Cities.HasAnyCitiesAsync(county.Id))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            await _appBLL.Counties.RemoveAsync(county.Id);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CountyExists(Guid id)
    {
        return _appBLL.Counties.Exists(id);
    }
}