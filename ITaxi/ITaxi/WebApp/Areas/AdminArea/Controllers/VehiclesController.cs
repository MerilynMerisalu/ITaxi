#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area vehicles controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class VehiclesController : Controller
{
    private readonly IAppBLL _appBLL;
    private readonly AppDbContext _context;
    /// <summary>
    /// Admin area vehicles controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public VehiclesController(IAppBLL appBLL, AppDbContext context)
    {
        _appBLL = appBLL;
        _context = context;
    }
    private string UserEmail => User.Identity!.Name!;

    // GET: AdminArea/Vehicles
    /// <summary>
    /// Admin area vehicles index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Vehicles.GettingOrderedVehiclesAsync();
        
        return View(res);
    }

    // GET: AdminArea/Vehicles/Details/5
    /// <summary>
    /// Admin area vehicle GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        vm.DoesElectricWheelchairFitInVehicle = vehicle.DoesElectricWheelchairFitInVehicle;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.CreatedBy = vehicle.CreatedBy = UserEmail;
        vm.CreatedAt = vehicle.CreatedAt;
        vm.UpdatedBy = vehicle.UpdatedBy = UserEmail;
        vm.UpdatedAt = vehicle.UpdatedAt;

        return View(vm);
    }

    /// <summary>
    /// Admin area vehicles set drop down list
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("AdminArea/Vehicles/SetDropDownList/{id}")]
    public async Task<IActionResult> SetDropDownList([FromRoute]Guid id)
    {
        // Using the EditRideTimeViewModel because I want to send through the SelectLists and Ids that have now changed
        var vm = new CreateEditVehicleViewModel();
       
        // Select the VehicleMarks for the currently selected VehicleMarkId
        
        var vehicleModels = await _appBLL.VehicleModels.GettingVehicleModelsByMarkIdAsync(id);
        vm.VehicleModels = new SelectList(vehicleModels, 
            nameof(VehicleModelDTO.Id), nameof(VehicleModelDTO.VehicleModelName));
        
        return Ok(vm);
    }

    // GET: AdminArea/Vehicles/Create
    /// <summary>
    /// Admin area vehicle GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditVehicleViewModel();
        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");
        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears());
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync()
            , nameof(VehicleMarkDTO.Id), nameof(VehicleMarkDTO.VehicleMarkName));
        vm.VehicleModels = new SelectList( await _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            //new VehicleModelDTO[0], // Deliberately empty until the user selects a Mark
            nameof(VehicleModelDTO.Id), nameof(VehicleModelDTO.VehicleModelName));
        vm.VehicleTypes = await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync();
            
            
        return View(vm);
    }

    // POST: AdminArea/Vehicles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area vehicle POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="vehicle">Vehicle</param>
    /// <returns>View</returns>
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

        vm.ManufactureYears = new SelectList(_appBLL.Vehicles.GettingManufactureYears(), 
            nameof(VehicleDTO.ManufactureYear));
        vm.VehicleTypes =  await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync();
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id),
            nameof(VehicleMarkDTO.VehicleMarkName), nameof(vehicle.VehicleMarkId));
        vm.VehicleModels = new SelectList(await _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            nameof(VehicleModelDTO.VehicleModelName), nameof(VehicleModelDTO.Id));

        return View(vm);
    }

    // GET: AdminArea/Vehicles/Edit/5
    /// <summary>
    /// Admin area vehicle GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vt = new VehicleTypeDTO();
        var vm = new CreateEditVehicleViewModel();
         
        if (id == null) return NotFound();
        
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();

        vm.Drivers = new SelectList(await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync(),
            nameof(DriverDTO.Id), "AppUser.LastAndFirstName");

        vm.VehicleTypes = await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync();
        
        vm.VehicleMarks = new SelectList(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync(),
            nameof(VehicleMarkDTO.Id),
            nameof(VehicleMarkDTO.VehicleMarkName));

        vm.VehicleModels = new SelectList(await
                _appBLL.VehicleModels.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(),
            // .GettingVehicleModelsByMarkIdAsync(vehicle.VehicleMarkId),
            nameof(VehicleModelDTO.Id),
            nameof(VehicleModelDTO.VehicleModelName));

        vm.Id = vehicle.Id;
        vm.DoesElectricWheelchairFitInVehicle = vehicle.DoesElectricWheelchairFitInVehicle;
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
    /// <summary>
    /// Admin area vehicle POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
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
                    vehicle.DoesElectricWheelchairFitInVehicle = vm.DoesElectricWheelchairFitInVehicle;
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
    /// <summary>
    /// Admin area vehicle GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
        vm.DoesElectricWheelchairFitInVehicle = vehicle.DoesElectricWheelchairFitInVehicle;
        vm.VehiclePlateNumber = vehicle.VehiclePlateNumber;
        vm.CreatedBy = vehicle.CreatedBy!;
        vm.CreatedAt = vehicle.CreatedAt;
        vm.UpdatedBy = vehicle.UpdatedBy!;
        vm.UpdatedAt = vehicle.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/Vehicles/Delete/5
    /// <summary>
    /// Admin area vehicle POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
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
            await _appBLL.Vehicles.RemoveAsync(vehicle.Id);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool VehicleExists(Guid id)
    {
        return _appBLL.Vehicles.Exists(id);
    }

    // GET: AdminArea/Vehicle/Gallery/5
    /// <summary>
    /// Admin area vehicle GET method gallery
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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