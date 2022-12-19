/*#nullable enable
using App.Contracts.DAL;
using App.Domain;
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
    private readonly IAppUnitOfWork _uow;

    public VehiclesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    private string UserEmail => User.Identity!.Name!;

    // GET: AdminArea/Vehicles
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        return View(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId, roleName));
    }

    // GET: AdminArea/Vehicles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();
        var vm = new DetailsDeleteVehicleViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
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

        vm.ManufactureYears = new SelectList(_uow.Vehicles.GettingManufactureYears());
        vm.VehicleMarks = new SelectList(await _uow.VehicleMarks.GetAllVehicleMarkOrderedAsync()
            , nameof(VehicleMark.Id), nameof(VehicleMark.VehicleMarkName));
        vm.VehicleModels = new SelectList(await _uow.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModel.Id), nameof(VehicleModel.VehicleModelName));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id),
            nameof(VehicleType.VehicleTypeName));
        return View(vm);
    }

    // POST: AdminArea/Vehicles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditVehicleViewModel vm, Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            var userId = User.GettingUserId();
            var driver = _uow.Drivers.SingleOrDefaultAsync(d => d!.AppUserId.Equals(userId)).Result;
            if (driver == null) return NotFound();

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
            _uow.Vehicles.Add(vehicle);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

#warning ManufactureYears needs checking

        vm.ManufactureYears = new SelectList(_uow.Vehicles.GettingManufactureYears(), nameof(Vehicle.ManufactureYear));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id),
            nameof(VehicleType.VehicleTypeName), nameof(vehicle.VehicleTypeId));
        vm.VehicleMarks = new SelectList(await _uow.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMark.Id),
            nameof(VehicleMark.VehicleMarkName), nameof(vehicle.VehicleMarkId));
        vm.VehicleModels = new SelectList(await _uow.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModel.VehicleModelName), nameof(VehicleModel.Id));

        return View(vm);
    }

    // GET: AdminArea/Vehicles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleViewModel();
        if (id == null) return NotFound();

        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id),
            nameof(VehicleType.VehicleTypeName));
        vm.VehicleMarks = new SelectList(await _uow.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMark.Id),
            nameof(VehicleMark.VehicleMarkName));

        vm.VehicleModels = new SelectList(await _uow.VehicleModels
                .GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModel.Id),
            nameof(VehicleModel.VehicleModelName));

        vm.Id = vehicle.Id;
        vm.ManufactureYears = new SelectList(_uow.Vehicles.GettingManufactureYears());
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
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        if (vehicle != null && id != vehicle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.Id = id;

                    var driver = _uow.Drivers.SingleOrDefaultAsync(d => d!.AppUserId.Equals(userId)).Result;
                    if (driver == null) return NotFound();

                    vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
                    vehicle.ManufactureYear = vm.ManufactureYear;
                    vehicle.VehicleAvailability = vm.VehicleAvailability;
                    vehicle.VehicleMarkId = vm.VehicleMarkId;
                    vehicle.VehicleModelId = vm.VehicleModelId;
                    vehicle.VehicleTypeId = vm.VehicleTypeId;
                    vehicle.NumberOfSeats = vm.NumberOfSeats;
                    vehicle.UpdatedBy = UserEmail;
                    vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Vehicles.Update(vehicle);
                    await _uow.SaveChangesAsync();
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
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
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
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id, userId, roleName);
        if (await _uow.Schedules.AnyAsync(s => vehicle != null && s != null && s.VehicleId.Equals(vehicle.Id))
            || await _uow.Bookings.AnyAsync(v => vehicle != null && v != null && v.VehicleId.Equals(vehicle.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (vehicle != null)
        {
            _uow.Vehicles.Remove(vehicle);
            await _uow.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleExists(Guid id)
    {
        return _uow.Vehicles.Exists(id);
    }

    // GET: DriverArea/Vehicle/Gallery/5
    public async Task<IActionResult> Gallery(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new GalleryViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vehicle = await _uow.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value, userId, roleName);
        if (vehicle == null) return NotFound();

        vm.VehicleIdentifier = vehicle.VehicleIdentifier;
        vm.Id = vehicle.Id;

        return View(vm);
    }
}*/