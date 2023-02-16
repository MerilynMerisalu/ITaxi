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
public class VehicleModelsController : Controller
{
    private readonly IAppBLL _appBLL;

    public VehicleModelsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/VehicleModels
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.VehicleModels.GetAllAsync();

        return View(res);
    }

    // GET: AdminArea/VehicleModels/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteVehicleModelViewModel();
        if (id == null) return NotFound();

        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id.Value);
        if (vehicleModel == null) return NotFound();

        vm.VehicleModelName = vehicleModel.VehicleModelName;
        vm.Id = vehicleModel.Id;
        vm.VehicleMarkName = vehicleModel.VehicleMark!.VehicleMarkName;
        vm.CreatedBy = vehicleModel.CreatedBy!;
        vm.CreatedAt = vehicleModel.CreatedAt;
        vm.UpdatedBy = vehicleModel.UpdatedBy!;
        vm.UpdatedAt = vehicleModel.UpdatedAt;
        return View(vm);
    }

    // GET: AdminArea/VehicleModels/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditVehicleModelViewModel();
        vm.VehicleMarks = new SelectList(
            await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id), nameof(VehicleMarkDTO.VehicleMarkName));
        return View(vm);
    }

    // POST: AdminArea/VehicleModels/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleModelViewModel vm, VehicleModelDTO vehicleModel)
    {
        if (ModelState.IsValid)
        {
            vehicleModel.Id = Guid.NewGuid();
            vehicleModel.VehicleModelName = vm.VehicleModelName;
            vehicleModel.VehicleMarkId = vm.VehicleMarkId;
            vehicleModel.CreatedBy = User.Identity!.Name;
            vehicleModel.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.VehicleModels.Add(vehicleModel);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleModels/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleModelViewModel();
        if (id == null) return NotFound();

        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id.Value);
        if (vehicleModel == null) return NotFound();

        vm.VehicleMarkId = vehicleModel.VehicleMark!.Id;
        vm.Id = vehicleModel.Id;
        vm.VehicleModelName = vehicleModel.VehicleModelName;
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id), nameof(VehicleMarkDTO.VehicleMarkName));
        return View(vm);
    }

    // POST: AdminArea/VehicleModels/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditVehicleModelViewModel vm)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id, noIncludes:true);
        if (vehicleModel != null && id != vehicleModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicleModel != null)
                {
                    vehicleModel.Id = id;
                    vehicleModel.VehicleMarkId = vm.VehicleMarkId;
                    vehicleModel.VehicleModelName = vm.VehicleModelName;
                    vehicleModel.UpdatedBy = User.Identity!.Name;
                    vehicleModel.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.VehicleModels.Update(vehicleModel);
                    await _appBLL.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicleModel != null && !VehicleModelExists(vehicleModel.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleModels/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteVehicleModelViewModel();
        if (id == null) return NotFound();

        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id.Value);
        if (vehicleModel == null) return NotFound();

        vm.Id = vehicleModel.Id;
        vm.VehicleMarkName = vehicleModel.VehicleMark!.VehicleMarkName;
        vm.VehicleModelName = vehicleModel.VehicleModelName;
        vm.CreatedBy = vehicleModel.CreatedBy!;
        vm.CreatedAt = vehicleModel.CreatedAt;
        vm.UpdatedBy = vehicleModel.UpdatedBy!;
        vm.UpdatedAt = vehicleModel.UpdatedAt;
        return View(vm);
    }

    // POST: AdminArea/VehicleModels/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels
            .FirstOrDefaultAsync(id);
        
        if (await _appBLL.Vehicles.HasAnyVehicleModelsAnyAsync(id))
            return Content("Entity cannot be deleted because it has dependent entities!");
        
        if (vehicleModel != null)
        {
            _appBLL.VehicleModels.Remove(vehicleModel);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleModelExists(Guid id)
    {
        return _appBLL.VehicleModels.Exists(id);
    }
}