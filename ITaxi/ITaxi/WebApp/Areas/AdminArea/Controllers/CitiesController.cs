#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class CitiesController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public CitiesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Cities
    public async Task<IActionResult> Index()
    {
        var res = await _uow.Cities.GetAllOrderedCitiesAsync();
#warning Should this be a repo method
        foreach (var city in res)
        {
            city.CreatedAt = city.CreatedAt.ToLocalTime();
            city.UpdatedAt = city.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Cities/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new DetailsDeleteCityViewModel();
        var city = await _uow.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.Id = city.Id;
        vm.CountyName = city.County!.CountyName;
        vm.CityName = city.CityName;
        vm.CreatedAt = city.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = city.CreatedBy!;
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = DateTime.Now.ToLocalTime().ToString("G");

        return View(vm);
    }

    // GET: AdminArea/Cities/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditCityViewModel();
        vm.Counties = new SelectList(await _uow.Counties.GetAllCountiesOrderedByCountyNameAsync(),
            nameof(County.Id), nameof(County.CountyName));
        return View(vm);
    }

    // POST: AdminArea/Cities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCityViewModel vm, City city)
    {
        if (ModelState.IsValid)
        {
            city.Id = Guid.NewGuid();
            city.CountyId = vm.CountyId;
            city.CityName = vm.CityName;
            city.CreatedBy = User.Identity!.Name;
            city.CreatedAt = DateTime.Now.ToUniversalTime();
            _uow.Cities.Add(city);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(vm);
    }

    // GET: AdminArea/Cities/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCityViewModel();
        if (id == null) return NotFound();

        var city = await _uow.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.Counties = new SelectList(await _uow.Counties.GetAllCountiesOrderedByCountyNameAsync(),
            nameof(County.Id), nameof(County.CountyName));
        vm.CityName = city.CityName;
        vm.CountyId = city.CountyId;
        

        return View(vm);
    }

    // POST: AdminArea/Cities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditCityViewModel vm)
    {
        var city = await _uow.Cities.FirstOrDefaultAsync(id);

        if (id != city!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                city.Id = id;
                city.CountyId = vm.CountyId;
                city.CityName = vm.CityName;
                city.UpdatedBy = User.Identity!.Name;
                city.UpdatedAt = DateTime.Now.ToUniversalTime();
                _uow.Cities.Update(city);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(city.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Cities/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCityViewModel();
        if (id == null) return NotFound();

        var city = await _uow.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.CityName = city.CityName;
        vm.CountyName = city.County!.CountyName;
        vm.CityName = city.CityName;
        vm.CreatedAt = city.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = city.CreatedBy!;
        vm.UpdatedBy = city.UpdatedBy!;
        vm.UpdatedAt = city.UpdatedAt.ToLocalTime().ToString("G");


        return View(vm);
    }

    // POST: AdminArea/Cities/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
#warning Ask if that can be improved
        var city = await _uow.Cities.FirstOrDefaultAsync(id);
        if (await _uow.Admins.AnyAsync(c => c != null && c.CityId.Equals(id)) ||
            await _uow.Bookings.AnyAsync(c => c != null && c.CityId.Equals(id)) ||
            await _uow.Drivers.AnyAsync(c => c != null && c.CityId.Equals(id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (city != null)
        {
            _uow.Cities.Remove(city);
            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CityExists(Guid id)
    {
        return _uow.Cities.Exists(id);
    }
}