#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area vehicle marks controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class VehicleMarksController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area vehicle marks controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public VehicleMarksController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/VehicleMarks
    /// <summary>
    /// Admin area vehicle marks controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync();
        return View(res);
    }

    // GET: AdminArea/VehicleMarks/Details/5
    /// <summary>
    /// Admin area vehicle marks controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteVehicleMarkViewModel();
        if (id == null) return NotFound();

        var vehicleMark = await _appBLL.VehicleMarks
            .FirstOrDefaultAsync(id.Value);
        if (vehicleMark == null) return NotFound();

        vm.VehicleMarkName = vehicleMark.VehicleMarkName;
        vm.Id = vehicleMark.Id;
        vm.CreatedBy = vehicleMark.CreatedBy!;
        vm.CreatedAt = vehicleMark.CreatedAt;
        vm.UpdatedBy = vehicleMark.UpdatedBy!;
        vm.UpdatedAt = vehicleMark.UpdatedAt;

        return View(vm);
    }

    // GET: AdminArea/VehicleMarks/Create
    /// <summary>
    /// Admin area vehicle marks controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditVehicleMarkViewModel();
        return View(vm);
    }

    // POST: AdminArea/VehicleMarks/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area vehicle marks controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="vehicleMark">Vehicle mark</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleMarkViewModel vm, VehicleMarkDTO vehicleMark)
    {
        if (ModelState.IsValid)
        {
            vehicleMark.Id = Guid.NewGuid();
            vehicleMark.VehicleMarkName = vm.VehicleMarkName;
            vehicleMark.CreatedBy = User.Identity!.Name;
            vehicleMark.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.VehicleMarks.Add(vehicleMark);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleMarks/Edit/5
    /// <summary>
    /// Admin area vehicle marks controller GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleMarkViewModel();
        if (id == null) return NotFound();

        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id.Value);
        if (vehicleMark?.VehicleMarkName != null) vm.VehicleMarkName = vehicleMark.VehicleMarkName;
        
        return View(vm);
    }

    // POST: AdminArea/VehicleMarks/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area vehicle marks controller POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditVehicleMarkViewModel vm)
    {
        var vehicleMark = await _appBLL.VehicleMarks
            .FirstOrDefaultAsync(id);
        if (vehicleMark != null && id != vehicleMark.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicleMark != null)
                {
                    vehicleMark.Id = id;
                    vehicleMark.VehicleMarkName = vm.VehicleMarkName;
                    vehicleMark.UpdatedBy = User.Identity!.Name!;
                    vehicleMark.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.VehicleMarks.Update(vehicleMark);
                    await _appBLL.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicleMark != null && !VehicleMarkExists(vehicleMark.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleMarks/Delete/5
    /// <summary>
    /// Admin area vehicle marks controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteVehicleMarkViewModel();
        if (id == null) return NotFound();

        var vehicleMark = await _appBLL.VehicleMarks
            .FirstOrDefaultAsync(id.Value);
        if (vehicleMark == null) return NotFound();

        vm.Id = vehicleMark.Id;
        vm.VehicleMarkName = vehicleMark.VehicleMarkName;
        vm.CreatedBy = vehicleMark.CreatedBy!;
        vm.CreatedAt = vehicleMark.CreatedAt;
        vm.UpdatedBy = vehicleMark.UpdatedBy!;
        vm.UpdatedAt = vehicleMark.UpdatedAt;
        
        return View(vm);
    }

    // POST: AdminArea/VehicleMarks/Delete/5
    /// <summary>
    /// Admin area vehicle marks controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks
            .FirstOrDefaultAsync(id);

        if (await _appBLL.VehicleModels.HasAnyVehicleMarksAsync(vehicleMark!.Id))
        {
            return Content("Entity cannot be deleted because it has dependent entities!");
        }
        
        await _appBLL.VehicleMarks.RemoveAsync(vehicleMark.Id);
        await _appBLL.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleMarkExists(Guid id)
    {
        return _appBLL.VehicleMarks.Exists(id);
    }
}