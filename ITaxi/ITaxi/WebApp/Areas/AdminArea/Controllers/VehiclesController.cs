#nullable disable
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
public class VehiclesController : Controller
{
    private readonly AppDbContext _context;

    public VehiclesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: AdminArea/Vehicles
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Vehicles
            .Include(v => v.Driver)
            .Include(v => v.Driver.AppUser)
            .Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType);
        return View(await appDbContext.ToListAsync());
    }

    // GET: AdminArea/Vehicles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();
        var vm = new DetailsDeleteVehicleViewModel();
        var vehicle = await _context.Vehicles
            .Include(v => v.Driver)
            .ThenInclude(d => d.AppUser)
            .Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType)
            .SingleOrDefaultAsync(m => m.Id == id);
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
        
        return View(vm);
    }

    // GET: AdminArea/Vehicles/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditVehicleViewModel();
        var manufactureYears = GettingManufactureYears();
        vm.ManufactureYears = GettingManufactureYearsSelectList(manufactureYears);
        vm.VehicleMarks = new SelectList(await _context.VehicleMarks.OrderBy(v => v.VehicleMarkName)
            .Select(v => new {v.Id, v.VehicleMarkName}).ToListAsync(), nameof(VehicleMark
            .Id), nameof(VehicleMark.VehicleMarkName));
        vm.VehicleModels = new SelectList(await _context.VehicleModels
                .Select(v => new {v.Id, v.VehicleModelName}).ToListAsync(),
            nameof(VehicleModel.Id), nameof(VehicleModel.VehicleModelName));
        vm.VehicleTypes = new SelectList(_context.VehicleTypes.Select(v => new {v.Id, v.VehicleTypeName}),
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
            vehicle.Id = Guid.NewGuid();

            vehicle.DriverId = await _context.Drivers.Select(d => d.Id).FirstOrDefaultAsync();
            vehicle.ManufactureYear = vm.ManufactureYear;
            vehicle.VehicleAvailability = vm.VehicleAvailability;
            vehicle.VehicleMarkId = vm.VehicleMarkId;
            vehicle.VehicleModelId = vm.VehicleModelId;
            vehicle.VehicleTypeId = vm.VehicleTypeId;
            vehicle.NumberOfSeats = vm.NumberOfSeats;
            vehicle.VehiclePlateNumber = vm.VehiclePlateNumber;
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Vehicles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditVehicleViewModel();
        if (id == null) return NotFound();

        var vehicle = await _context.Vehicles.
            Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType)
            .SingleOrDefaultAsync(v => v.Id.Equals(id));
        if (vehicle == null) return NotFound();
        var manufactureYears = GettingManufactureYears();
        vm.ManufactureYears = GettingManufactureYearsSelectList(manufactureYears);
        vm.VehicleTypes = new SelectList(await _context.VehicleTypes
                .Select(v => new {v.Id, v.VehicleTypeName}).ToListAsync(),
            nameof(VehicleType.Id),
            nameof(VehicleType.VehicleTypeName));
        vm.VehicleMarks = new SelectList(await _context.VehicleMarks
                .Select(v => new {v.Id, v.VehicleMarkName}).ToListAsync(),
            nameof(VehicleMark.Id),
            nameof(VehicleMark.VehicleMarkName));

        vm.VehicleModels = new SelectList(await _context.VehicleModels
                .Select(v => new {v.Id, v.VehicleModelName}).ToListAsync(),
            nameof(VehicleModel.Id),
            nameof(VehicleModel.VehicleModelName));
        
        vm.Id = vehicle.Id;
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
        var vehicle = await _context.Vehicles.SingleAsync(v => v.Id.Equals(id));
        if (id != vehicle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                vehicle.Id = id;
                vehicle.DriverId = _context.Drivers.SingleAsync(d => d.Id.Equals(vehicle.DriverId))
                    .Result.Id;
                vehicle.ManufactureYear = vm.ManufactureYear;
                vehicle.VehicleAvailability = vm.VehicleAvailability;
                vehicle.VehicleMarkId = vm.VehicleMarkId;
                vehicle.VehicleModelId = vm.VehicleModelId;
                vehicle.VehicleTypeId = vm.VehicleTypeId;
                vehicle.NumberOfSeats = vm.NumberOfSeats;
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vehicle.Id))
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
        var vehicle = await _context.Vehicles.
            Include(v => v.Driver)
            .ThenInclude(d => d.AppUser).
            Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType)
            .SingleOrDefaultAsync(v => v.Id.Equals(id));
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

        return View(vm);
    }

    // POST: AdminArea/Vehicles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var vehicle = await _context.Vehicles.SingleOrDefaultAsync(v => v.Id.Equals(id));
        if (await _context.Schedules.AnyAsync(s => s.VehicleId.Equals(vehicle.Id)) 
            || await _context.Bookings.AnyAsync(v => v.VehicleId.Equals(vehicle.Id)))
        {
            return Content("Entity cannot be deleted because it has dependent entities!");
        }
        if (vehicle != null) _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VehicleExists(Guid id)
    {
        return _context.Vehicles.Any(e => e.Id == id);
    }

    /// <summary>
    ///     Creates a list of manufacture years.
    /// </summary>
    /// <returns>Select list years</returns>
    private List<int> GettingManufactureYears()
    {
        var years = new List<int>();

        for (var i = 6; i > 0; i--)
        {
            var year = DateTime.Today.AddYears(-i).Year;
            years.Add(year);
        }

        years.Reverse();

        return years;
    }
    /// <summary>
    /// Converting years to a selectList
    /// </summary>
    /// <param name="years">List of years</param>
    /// <returns></returns>
    private SelectList GettingManufactureYearsSelectList(ICollection<int> years)
    {
        var manufactureYears = new SelectList(years);
        return manufactureYears;
    }
}