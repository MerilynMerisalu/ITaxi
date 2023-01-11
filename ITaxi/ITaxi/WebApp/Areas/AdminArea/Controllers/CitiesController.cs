#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class CitiesController : Controller
{
    private readonly IAppBLL _appBLL;

    public CitiesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Cities
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Cities.GetAllOrderedCitiesAsync();
#warning Should this be a repo method
        

        return View(res);
    }

    // GET: AdminArea/Cities/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new DetailsDeleteCityViewModel();
        var city = await _appBLL.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.Id = city.Id;
        vm.CountyName = city.County!.CountyName;
        vm.CityName = city.CityName;
        vm.CreatedAt = city.CreatedAt;
        vm.CreatedBy = city.CreatedBy!;
        vm.UpdatedBy = User.Identity!.Name!;
        vm.UpdatedAt = city.UpdatedAt;
        

        return View(vm);
    }

    // GET: AdminArea/Cities/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditCityViewModel();

        vm.Counties = new SelectList(await _appBLL.Counties.GetAllAsync(),
            nameof(CountyDTO.Id), nameof(CountyDTO.CountyName));
        return View(vm);
    }

    // POST: AdminArea/Cities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCityViewModel vm, CityDTO city)
    {
        if (ModelState.IsValid)
        {
            city.Id = Guid.NewGuid();
            city.CountyId = vm.CountyId;
            city.CityName = vm.CityName;
            city.CreatedBy = User.Identity!.Name;
            city.CreatedAt = DateTime.Now;
            _appBLL.Cities.Add(city);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(vm);
    }

    // GET: AdminArea/Cities/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCityViewModel();
        if (id == null) return NotFound();

        var city = await _appBLL.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.Counties = new SelectList(await _appBLL.Counties.GetAllAsync(),
            nameof(CountyDTO.Id), nameof(CountyDTO.CountyName));
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
        var city = await _appBLL.Cities.FirstOrDefaultAsync(id);

        if (id != city!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                city.Id = id;
                city.CountyId = vm.CountyId;
                city.County = await _appBLL.Counties.FirstOrDefaultAsync(vm.CountyId);
                city.CityName = vm.CityName;
                city.UpdatedBy = User.Identity!.Name;
                city.UpdatedAt = DateTime.Now;
                _appBLL.Cities.Update(city);
                await _appBLL.SaveChangesAsync();
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

        var city = await _appBLL.Cities.FirstOrDefaultAsync(id.Value);
        if (city == null) return NotFound();

        vm.CityName = city.CityName;
        vm.CountyName = city.County!.CountyName;
        vm.CityName = city.CityName;
        vm.CreatedAt = city.CreatedAt;
        vm.CreatedBy = city.CreatedBy!;
        vm.UpdatedBy = city.UpdatedBy!;
        vm.UpdatedAt = city.UpdatedAt;


        return View(vm);
    }

    // POST: AdminArea/Cities/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
#warning Ask if that can be improved
        var city = await _appBLL.Cities.FirstOrDefaultAsync(id);
        //if (await _appBLL.Admins.HasAnyByCityIdAsync(id) ||
        //    await _appBLL.Bookings.HasAnyByCityIdAsync(id) ||
        //    await _appBLL.Drivers.HasAnyByCityIdAsync(id)
        //    return Content("Entity cannot be deleted because it has dependent entities!");

        if (city != null)
        {
            _appBLL.Cities.Remove(city);
            await _appBLL.SaveChangesAsync();
        }




        return RedirectToAction(nameof(Index));
    }

    private bool CityExists(Guid id)
    {
        return _appBLL.Cities.Exists(id);
    }
}