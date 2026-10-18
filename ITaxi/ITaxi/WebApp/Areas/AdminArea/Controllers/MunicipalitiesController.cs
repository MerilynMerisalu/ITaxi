#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Enum.Enum;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area cities controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class MunicipalitiesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area cities controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public MunicipalitiesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Cities
    /// <summary>
    /// Admin area cities index
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Municipalities.GetAllOrderedMunipalitiesAsync(showIgnored: true, showDeleted: true);

        return View(res);
    }

    // GET: AdminArea/Cities/Details/5
    /// <summary>
    /// Admin area cities details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new DetailsDeleteCityViewModel();
        var municipality = await _appBLL.Municipalities.FirstOrDefaultAsync(id.Value, showIgnored: true);
        if (municipality == null) return NotFound();

        vm.Id = municipality.Id;
        vm.CountyName = municipality.County!.CountyName;
        vm.CityName = municipality.Name;
        vm.CreatedAt = municipality.CreatedAt;
        vm.CreatedBy = municipality.CreatedBy!;
        vm.UpdatedBy = municipality.UpdatedBy;
        vm.UpdatedAt = municipality.UpdatedAt;
        
        return View(vm);
    }

    // GET: AdminArea/Cities/Create
    /// <summary>
    /// Admin area municipality create
    /// </summary>
    /// <returns>View model</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditCityViewModel();

        vm.Counties = new SelectList(await _appBLL.Counties.GetAllCountiesOrderedByCountyNameAsync(showIgnored: false),
            nameof(CountyDTO.Id), nameof(CountyDTO.CountyName));
        return View(vm);
    }

    // POST: AdminArea/Cities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area municipality create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="municipality">City</param>
    /// <returns>View with view model</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCityViewModel vm, MunicipalityDTO municipality)
    {
        if (ModelState.IsValid)
        {
            municipality.Id = Guid.NewGuid();
            municipality.CountyId = vm.CountyId;
            municipality.Name = vm.CityName;
            municipality.DataOrigin = DataOrigin.Manual;
            
            _appBLL.Municipalities.Add(municipality);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View(vm);
    }

    // GET: AdminArea/Cities/Edit/5
    /// <summary>
    /// Admin area municipality edit GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View with view model</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCityViewModel();
        if (id == null) return NotFound();

        var municipality = await _appBLL.Municipalities.GetMunicipalityByIdAsync(id: id.Value, showIgnored: true);
        if (municipality == null) return NotFound();
        if (municipality.County!.IsIgnored) 
            vm.IsCountyIgnored = true;
        vm.Counties = new SelectList(await _appBLL.Counties.GetAllAsync(),
            nameof(CountyDTO.Id), nameof(CountyDTO.CountyName));
        vm.CityName = municipality.Name;
        vm.CountyId = municipality.CountyId;

        return View(vm);
    }

    // POST: AdminArea/Cities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area municipality edit POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View with view model</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditCityViewModel vm)
    {
        var municipality = await _appBLL.Municipalities.FirstOrDefaultMunicipalityWithoutCountyAsync(id, showIgnored: true);

        if (id != municipality!.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                municipality.Id = id;
                municipality.Name = vm.CityName;
                if (municipality.CountyId != Guid.Empty)
                {
                    municipality.CountyId = vm.CountyId;
                }
                _appBLL.Municipalities.Update(municipality);
                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(municipality.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Cities/Delete/5
    /// <summary>
    /// Admin area municipality delete GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View with view model</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCityViewModel();
        if (id == null) return NotFound();

        var municipality = await _appBLL.Municipalities.GetMunicipalityByIdAsync(id: id.Value, showIgnored: true);
        if (municipality == null) return NotFound();
        if (municipality.County!.IsIgnored)
            vm.IsCountyIgnored = true;

        vm.CityName = municipality.Name;
        vm.CountyName = municipality.County!.CountyName;
        
        vm.CreatedAt = municipality.CreatedAt;
        vm.CreatedBy = municipality.CreatedBy!;
        vm.UpdatedBy = municipality.UpdatedBy!;
        vm.UpdatedAt = municipality.UpdatedAt;
        
        return View(vm);
    }

    // POST: AdminArea/Cities/Delete/5
    /// <summary>
    /// Admin area municipality delete POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect user to cities index page</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var city = await _appBLL.Municipalities.GetMunicipalityByIdAsync(id: id, showIgnored: true);
        if (city != null)
        {
            try
            {
                 _appBLL.Municipalities.Remove(city.Id, hardDelete: false, showIgnored: true);
            }
            catch (ApplicationException e)
            {
                return Content(e.Message);
            }
            await _appBLL.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }
    

    private bool CityExists(Guid id)
    {
        return _appBLL.Municipalities.Exists(id);
    }

    public async Task<IActionResult> ShowHide(Guid id)
    {
        var result = await _appBLL.Municipalities.ToggleIsIgnoredAsync(id: id, showIgnored: true);
        return RedirectToAction(nameof(Index));
    }
}