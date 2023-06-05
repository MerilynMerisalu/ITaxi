#nullable enable
using App.Contracts.BLL;
using App.BLL.DTO.AdminArea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area vehicle types controller
/// </summary>
[Authorize(Roles = "Admin")]
[Area(nameof(AdminArea))]
public class VehicleTypesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area vehicle types controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public VehicleTypesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/VehicleTypes
    /// <summary>
    /// Admin area vehicle types index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync();
        
        return View(res);
    }

    // GET: AdminArea/VehicleTypes/Details/5
    /// <summary>
    /// Admin area vehicle type GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteVehicleTypeViewModel();
        if (id == null) return NotFound();

        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id.Value);
        if (vehicleType == null) return NotFound();

        vm.VehicleTypeName = vehicleType.VehicleTypeName;
        vm.Id = vehicleType.Id;
        vm.CreatedBy = vehicleType.CreatedBy!;
        vm.CreatedAt = vehicleType.CreatedAt;
        vm.UpdatedBy = vehicleType.UpdatedBy!;
        vm.UpdatedAt = vehicleType.UpdatedAt;
        return View(vm);
    }

    // GET: AdminArea/VehicleTypes/Create
    /// <summary>
    /// Admin area vehicle type GET method create
    /// </summary>
    /// <returns>View</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditVehicleTypeViewModel();
        return View(vm);
    }

    // POST: AdminArea/VehicleTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area vehicle type POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleTypeViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var vehicleType = new VehicleTypeDTO();
            vehicleType.Id = Guid.NewGuid();
            vehicleType.VehicleTypeName = vm.VehicleTypeName;
            vehicleType.CreatedBy = User.Identity!.Name;
            vehicleType.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.VehicleTypes.Add(vehicleType);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleTypes/Edit/5
    /// <summary>
    /// Admin area vehicle type GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleTypeViewModel();

        if (id == null) return NotFound();

        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id.Value);
        if (vehicleType != null)
        {
            vm.VehicleTypeName = vehicleType.VehicleTypeName;
            vm.Id = vehicleType.Id;
        }

        return View(vm);
    }

    // POST: AdminArea/VehicleTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area vehicle type POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditVehicleTypeViewModel vm)
    {
        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);

        if (vehicleType != null && id != vehicleType.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicleType != null)
                {
                    vehicleType.Id = vm.Id;
                    vehicleType.VehicleTypeName.SetTranslation(vm.VehicleTypeName);
                    vehicleType.UpdatedBy = User.Identity!.Name;
                    vehicleType.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.VehicleTypes.Update(vehicleType);
                    await _appBLL.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicleType != null && !VehicleTypeExists(vehicleType.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/VehicleTypes/Delete/5
    /// <summary>
    /// Admin area vehicle type GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteVehicleTypeViewModel();
        if (id == null) return NotFound();

        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id.Value);
        if (vehicleType == null) return NotFound();

        vm.Id = vehicleType.Id;
        vm.VehicleTypeName = vehicleType.VehicleTypeName;
        vm.CreatedBy = vehicleType.CreatedBy!;
        vm.CreatedAt = vehicleType.CreatedAt;
        vm.UpdatedBy = vehicleType.UpdatedBy!;
        vm.UpdatedAt = vehicleType.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/VehicleTypes/Delete/5
    /// <summary>
    /// Admin area vehicle type POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);
        if (vehicleType != null && (await _appBLL.VehicleTypes.HasVehiclesAnyAsync(vehicleType.Id)
                                    || await _appBLL.VehicleTypes.HasBookingsAnyAsync(vehicleType.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (vehicleType != null)
        {
            await _appBLL.VehicleTypes.RemoveAsync(vehicleType.Id);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleTypeExists(Guid id)
    {
        return _appBLL.VehicleTypes.Exists(id);
    }
}