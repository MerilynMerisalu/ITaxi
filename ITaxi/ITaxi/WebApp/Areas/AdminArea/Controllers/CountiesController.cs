#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class CountiesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public CountiesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Counties
    public async Task<IActionResult> Index()
    {
#warning Should this be a repo method
        var res = await _uow.Counties.GetAllCountiesOrderedByCountyNameAsync();
        foreach (var county in res)
        {
            county.CreatedAt = county.CreatedAt.ToLocalTime();
            county.UpdatedAt = county.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Counties/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCountyViewModel();
        if (id == null) return NotFound();

        var county = await _uow.Counties.FirstOrDefaultAsync(id.Value);
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
    public async Task<IActionResult> Create(CreateEditCountyViewModel vm, County county)
    {
        if (ModelState.IsValid)
        {
            county.Id = Guid.NewGuid();
            county.CountyName = vm.CountyName;
            county.CreatedBy = User.Identity!.Name;
            county.CreatedAt = DateTime.Now.ToUniversalTime();
            _uow.Counties.Add(county);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Counties/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCountyViewModel();
        if (id == null) return NotFound();

        var county = await _uow.Counties.FirstOrDefaultAsync(id.Value);

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
        var county = await _uow.Counties.FirstOrDefaultAsync(id);
        if (ModelState.IsValid)
            if (county != null && county.Id.Equals(id))
            {
                try
                {
                    county.CountyName = vm.CountyName;
                    county.UpdatedBy = User.Identity!.Name!;
                    county.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Counties.Update(county);
                    await _uow.SaveChangesAsync();
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

        var county = await _uow.Counties.FirstOrDefaultAsync(id.Value);
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
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var county = await _uow.Counties.FirstOrDefaultAsync(id);
        if (await _uow.Cities.AnyAsync(c => c!.CountyId.Equals(id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (county != null)
        {
            _uow.Counties.Remove(county);
            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CountyExists(Guid id)
    {
        return _uow.Counties.Exists(id);
    }
}