#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;

using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class CountiesController : Controller
{
    private readonly IAppBLL _appBLL;

    public CountiesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Counties
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Counties.GetAllCountiesOrderedByCountyNameAsync();
        
       
        return View(res);
    }

    // GET: AdminArea/Counties/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCountyViewModel();
        if (id == null) return NotFound();

        var county = await _appBLL.Counties.FirstOrDefaultAsync(id.Value);
        if (county == null) return NotFound();

        vm.CountyName = county.CountyName;
        vm.CreatedAt = county.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = county.CreatedBy!;
        vm.UpdatedAt = county.UpdatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = county.UpdatedBy!;
        vm.Id = county.Id;

        return View(vm);
    }

    // GET: AdminArea/Counties/Create

    public IActionResult Create()
    {
        var vm = new CreateEditCountyViewModel();
        return View(vm);
    }

    // POST: AdminArea/Counties/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCountyViewModel vm, CountyDTO county)
    {
        if (ModelState.IsValid)
        {
            county.Id = Guid.NewGuid();
            county.CountyName = vm.CountyName;
            county.CreatedBy = User.Identity!.Name;
            county.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Counties.Add(county);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Counties/Edit/5
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditCountyViewModel vm)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (ModelState.IsValid)
            if (county != null && county.Id.Equals(id))
            {
                try
                {
                    county.CountyName = vm.CountyName;
                    county.UpdatedBy = User.Identity!.Name!;
                    county.UpdatedAt = DateTime.Now.ToUniversalTime();
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
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCountyViewModel();
        if (id == null) return NotFound();

        var county = await _appBLL.Counties.FirstOrDefaultAsync(id.Value);
        if (county == null) return NotFound();

        vm.CountyName = county.CountyName;
        vm.CreatedAt = county.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = county.CreatedBy ?? "";
        vm.UpdatedAt = county.UpdatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = county.UpdatedBy ?? "";

        return View(vm);
    }

    // POST: AdminArea/Counties/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        //if (await _uow.Counties.HasCities(id))
        //    return Content("Entity cannot be deleted because it has dependent entities!");
        
        await _appBLL.Counties.RemoveAsync(id);
        await _appBLL.SaveChangesAsync();
       
        return RedirectToAction(nameof(Index));
    }

    private bool CountyExists(Guid id)
    {
        return _appBLL.Counties.Exists(id);
    }
}