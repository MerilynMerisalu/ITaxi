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

[Authorize(Roles = "Admin, Driver")]
[Area(nameof(DriverArea))]
public class VehiclesController : Controller
{
    private readonly IAppBLL _appBLL;

    public VehiclesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    private string UserEmail => User.Identity!.Name!;

    // GET: AdminArea/Vehicles
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync(userId, roleName);
        return View(res);
    }

    // GET: AdminArea/Vehicles/Details/5
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

    // GET: AdminArea/Vehicles/Create
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleViewModel vm, VehicleDTO vehicle)
    {
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            var driver = await _appBLL.Drivers.GettingDriverByVehicleAsync(userId);

            vehicle.Id = Guid.NewGuid();
            vehicle.DriverId = driver.Id;
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

#warning ManufactureYears needs checking

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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditVehicleViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        if (vehicle != null && id != vehicle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.Id = id;

                    var driver = await _appBLL.Drivers.GettingDriverByAppUserIdAsync(vehicle.Driver!.AppUserId);

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
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        /*if (await _appBLL.Schedules.AnyAsync(s => vehicle != null && s != null && s.VehicleId.Equals(vehicle.Id))
            || await _appBLL.Bookings.AnyAsync(v => vehicle != null && v != null && v.VehicleId.Equals(vehicle.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");*/

        if (vehicle != null)
        {
            _appBLL.Vehicles.Remove(vehicle);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }

    // GET: DriverArea/Vehicle/Gallery/5
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