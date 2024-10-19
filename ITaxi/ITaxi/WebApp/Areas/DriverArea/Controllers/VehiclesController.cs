#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.DriverArea.ViewModels;

namespace WebApp.Areas.DriverArea.Controllers;

/// <summary>
/// Driver area vehicles controller
/// </summary>
[Authorize(Roles = "Admin, Driver")]
[Area(nameof(DriverArea))]
public class VehiclesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Driver area vehicles controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public VehiclesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }
    private string UserEmail => User.Identity!.Name!;

    // GET: AdminArea/Vehicles
    /// <summary>
    /// Driver area vehicles index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName);
        return View(res);
    }

    // GET: AdminArea/Vehicles/Details/5
    /// <summary>
    /// Driver area vehicles GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();
        var vm = new DetailsDeleteVehicleViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.Id = id;
        vm.VehicleType = vehicle.VehicleType!.VehicleTypeName;

        vm.VehicleMark = vehicle.VehicleMark!.VehicleMarkName;
        vm.VehicleModel = vehicle.VehicleModel!.VehicleModelName;
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;

        return View(vm);
    }
    /// <summary>
    /// driver area vehicles set drop down list
    /// </summary>
    /// <param name="id">Vehicle mark id</param>
    /// <returns>Status 200 OK</returns>
    
    [HttpPost("DriverArea/Vehicles/SetDropDownList/{id}")]
    public async Task<IActionResult> SetTheDropDownList([FromRoute] Guid id)
    {
        var vm = new CreateEditVehicleViewModel();
        var vehicleModels = await _appBLL.VehicleModels.GettingVehicleModelsByMarkIdAsync(id);
        vm.VehicleModels = new SelectList(vehicleModels, nameof(VehicleModelDTO.Id), nameof(VehicleModelDTO.VehicleModelName));
        return Ok(vm);
    }

    // GET: AdminArea/Vehicles/Create
    /// <summary>
    /// Driver area vehicles GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditVehicleViewModel();

        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears());
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync()
            , nameof(VehicleMarkDTO.Id), nameof(VehicleMarkDTO.VehicleMarkName));
        vm.VehicleModels = new SelectList(await _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModelDTO.Id), nameof(VehicleModelDTO.VehicleModelName));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id),
            nameof(VehicleTypeDTO.VehicleTypeName));
        return View(vm);
    }

    // POST: AdminArea/Vehicles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Driver area vehicles POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="vehicle">Vehicles</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleViewModel vm, VehicleDTO vehicle)
    {
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            var driver = await _appBLL.Drivers.GettingDriverByVehicleAsync(userId);

            vehicle.Id = Guid.NewGuid();
            vehicle.DriverId = _appBLL.Drivers.GettingDriverByAppUserIdAsync(userId).Result.Id;
            vehicle.ManufactureYear = vm.ManufactureYear;
            vehicle.VehicleAvailability = vm.VehicleAvailability;
            vehicle.VehicleMarkId = vm.VehicleMarkId;
            vehicle.VehicleModelId = vm.VehicleModelId;
            vehicle.VehicleTypeId = vm.VehicleTypeId;
            vehicle.NumberOfSeats = vm.NumberOfSeats;
            vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
            vehicle.CreatedBy = UserEmail;
            vehicle.CreatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Vehicles.Add(vehicle);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears(), 
            nameof(VehicleDTO.ManufactureYear));
        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id),
            nameof(VehicleTypeDTO.VehicleTypeName), nameof(vehicle.VehicleTypeId));
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id),
            nameof(VehicleMarkDTO.VehicleMarkName), nameof(vehicle.VehicleMarkId));
        vm.VehicleModels = new SelectList(await _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModelDTO.VehicleModelName), nameof(VehicleModelDTO.Id));

        return View(vm);
    }

    // GET: AdminArea/Vehicles/Edit/5
    /// <summary>
    /// Driver area vehicles GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id),
            nameof(VehicleTypeDTO.VehicleTypeName));
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id),
            nameof(VehicleMarkDTO.VehicleMarkName));

        vm.VehicleModels = new SelectList(await _appBLL.VehicleModels
                .GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModelDTO.Id),
            nameof(VehicleModelDTO.VehicleModelName));

        vm.Id = vehicle.Id;
        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears());
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehicleTypeId = vehicle.VehicleTypeId;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.VehicleMarkId = vehicle.VehicleMarkId;
        vm.VehicleModelId = vehicle.VehicleModelId;
        return View(vm);
    }

    // POST: AdminArea/Vehicles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Driver area vehicles POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditVehicleViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName);
        if (vehicle != null && id != vehicle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.Id = id;

                    vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
                    vehicle.ManufactureYear = vm.ManufactureYear;
                    vehicle.VehicleAvailability = vm.VehicleAvailability;
                    vehicle.VehicleMarkId = vm.VehicleMarkId;
                    vehicle.VehicleModelId = vm.VehicleModelId;
                    vehicle.VehicleTypeId = vm.VehicleTypeId;
                    vehicle.NumberOfSeats = vm.NumberOfSeats;
                    vehicle.UpdatedBy = UserEmail;
                    vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Vehicles.Update(vehicle);
                    await _appBLL.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicle != null && !VehicleExists(vehicle.Id))
                    return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        
        return View(vm);
    }

    // GET: AdminArea/Vehicles/Delete/5
    /// <summary>
    /// Driver area vehicles GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new DetailsDeleteVehicleViewModel();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.Id = id;
        vm.VehicleType = vehicle.VehicleType!.VehicleTypeName;
        vm.VehicleMark = vehicle.VehicleMark!.VehicleMarkName;
        vm.VehicleModel = vehicle.VehicleModel!.VehicleModelName;
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;

        return View(vm);
    }

    // POST: AdminArea/Vehicles/Delete/5
    /// <summary>
    /// Driver area vehicles POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        if (vehicle != null)
        {
            if (await _appBLL.Vehicles.HasAnySchedulesAnyAsync(vehicle.Id) || await _appBLL.Vehicles.HasAnyBookingsAnyAsync(vehicle.Id))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            await _appBLL.Vehicles.RemoveAsync(vehicle.Id);
            await _appBLL.SaveChangesAsync();

        }
        return RedirectToAction(nameof(Index));
    }

    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }

    // GET: DriverArea/Vehicle/Gallery/5
    /// <summary>
    /// Driver area vehicles GET method gallery
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Gallery(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new GalleryViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.VehicleIdentifier = vehicle.VehicleIdentifier;
        vm.Id = vehicle.Id;

        return View(vm);
    }
}