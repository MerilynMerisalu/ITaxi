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
        
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync();
        
        return View(res);
    }

    // GET: AdminArea/Vehicles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();
        
        var vm = new DetailsDeleteVehicleViewModel();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();
        vm.DriverFullName = vehicle.Driver!.AppUser!.FirstAndLastName;
        vm.Id = id;
        vm.VehicleType = vehicle.VehicleType!.VehicleTypeName;
        vm.VehicleMark = vehicle.VehicleMark!.VehicleMarkName;
        vm.VehicleModel = vehicle.VehicleModel!.VehicleModelName;
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.CreatedBy = vehicle.CreatedBy = UserEmail;
        vm.CreatedAt = vehicle.CreatedAt;
        vm.UpdatedBy = vehicle.UpdatedBy = UserEmail;
        vm.UpdatedAt = vehicle.UpdatedAt;

        return View(vm);
    }

    [HttpPost("AdminArea/Vehicles/SetDropDownList/{id}")]
    public async Task<IActionResult> SetDropDownList([FromRoute]Guid id)
    {
        // Use the EditRideTimeViewModel because we want to send through the SelectLists and Ids that have now changed
        var vm = new CreateEditVehicleViewModel();
       
        // Select the VehicleMarks for the currently selected VehicleMarkId
        
        var vehicleModels = await _appBLL.VehicleModels.GettingVehicleModelsByMarkIdAsync(id);
        vm.VehicleModels = new SelectList(vehicleModels, 
            nameof(VehicleModelDTO.Id), nameof(VehicleModelDTO.VehicleModelName));
        
        return Ok(vm);
    }

    // GET: AdminArea/Vehicles/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditVehicleViewModel();

        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");
        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears());
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync()
            , nameof(VehicleMarkDTO.Id), nameof(VehicleMarkDTO.VehicleMarkName));
        vm.VehicleModels = new SelectList( await _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            //new VehicleModelDTO[0], // Deliberately empty until the user selects a Mark
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
            vehicle.Id = Guid.NewGuid();

            vehicle.DriverId = vm.DriverId;
            if (vehicle.Driver != null) vehicle.DriverId = vehicle.Driver.Id;
            vehicle.ManufactureYear = vm.ManufactureYear;
            vehicle.VehicleAvailability = vm.VehicleAvailability;
            vehicle.VehicleMarkId = vm.VehicleMarkId;
            vehicle.VehicleModelId = vm.VehicleModelId;
            vehicle.VehicleTypeId = vm.VehicleTypeId;
            vehicle.NumberOfSeats = vm.NumberOfSeats;
            vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
            vehicle.CreatedBy = User.Identity!.Name;
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

        
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();

        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
#warning "Magic string" code smell, fix it
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");

        vm.VehicleTypes = new SelectList(await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleTypeDTO.Id),
            nameof(VehicleTypeDTO.VehicleTypeName));
        
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id),
            nameof(VehicleMarkDTO.VehicleMarkName));

        vm.VehicleModels = new SelectList(await
                _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            // .GettingVehicleModelsByMarkIdAsync(vehicle.VehicleMarkId),
            nameof(VehicleModelDTO.Id),
            nameof(VehicleModelDTO.VehicleModelName));


        vm.Id = vehicle.Id;
        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears());
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehicleTypeId = vehicle.VehicleTypeId;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.DriverId = vehicle.DriverId;
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
        var vehicle = await _appBLL.Vehicles.FirstOrDefaultAsync(id, noIncludes:true);
        if (vehicle != null && id != vehicle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.Id = id;
                    vehicle.DriverId = vm.DriverId;
                    vehicle.ManufactureYear = vm.ManufactureYear;
                    vehicle.VehicleAvailability = vm.VehicleAvailability;
                    vehicle.VehicleMarkId = vm.VehicleMarkId;
                    vehicle.VehicleModelId = vm.VehicleModelId;
                    vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
                    vehicle.VehicleTypeId = vm.VehicleTypeId;
                    vehicle.NumberOfSeats = vm.NumberOfSeats;
                    vehicle.UpdatedBy = User.Identity!.Name;
                    vehicle.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Vehicles.Update(vehicle);
                    await _appBLL.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (vehicle != null && !VehicleExists(vehicle.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Vehicles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();
        
        var vm = new DetailsDeleteVehicleViewModel();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();
        vm.DriverFullName = vehicle.Driver!.AppUser!.FirstAndLastName;
        vm.Id = id;
        vm.VehicleType = vehicle.VehicleType!.VehicleTypeName;
        vm.VehicleMark = vehicle.VehicleMark!.VehicleMarkName;
        vm.VehicleModel = vehicle.VehicleModel!.VehicleModelName;
        vm.ManufactureYear = vehicle.ManufactureYear;
        vm.VehicleAvailability = vehicle.VehicleAvailability;
        vm.NumberOfSeats = vehicle.NumberOfSeats;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.CreatedBy = vehicle.CreatedBy!;
        vm.CreatedAt = vehicle.CreatedAt;
        vm.UpdatedBy = vehicle.UpdatedBy!;
        vm.UpdatedAt = vehicle.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/Vehicles/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var vehicle = await _appBLL.Vehicles.FirstOrDefaultAsync(id);
        if (vehicle != null && (await _appBLL.Vehicles.HasAnySchedulesAnyAsync(vehicle.Id) || 
                                await _appBLL.Vehicles.HasAnyBookingsAnyAsync(vehicle.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

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

    // GET: AdminArea/Vehicle/Gallery/5
    public async Task<IActionResult> Gallery(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new GalleryViewModel();
        
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();

        vm.VehicleIdentifier = vehicle.VehicleIdentifier;
        vm.Id = vehicle.Id;

        return View(vm);
    }
}