#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class VehicleTypesController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/VehicleTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleTypes.ToListAsync());
        }

        // GET: AdminArea/VehicleTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleTypeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            vm.VehicleTypeName = vehicleType.VehicleTypeName;
            vm.Id = vehicleType.Id;
            return View(vm);
        }

        // GET: AdminArea/VehicleTypes/Create
        public IActionResult Create()
        {
            var vm = new CreateEditVehicleTypeViewModel();
            return View(vm);
        }

        // POST: AdminArea/VehicleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditVehicleTypeViewModel vm, VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                vehicleType.Id = Guid.NewGuid();
                vehicleType.VehicleTypeName = vm.VehicleTypeName;
                _context.Add(vehicleType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/VehicleTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditVehicleTypeViewModel();
            
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes.SingleAsync(v => v.Id.Equals(id));
            vm.VehicleTypeName = vehicleType.VehicleTypeName;
            vm.Id = vehicleType.Id;
            return View(vm);
        }

        // POST: AdminArea/VehicleTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditVehicleTypeViewModel vm)
        {
            var vehicleType = await _context.VehicleTypes.SingleAsync(v => v.Id.Equals(id));
            
            if (id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleType.Id = vm.Id;
                    vehicleType.VehicleTypeName = vm.VehicleTypeName;
                    _context.Update(vehicleType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/VehicleTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteVehicleTypeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            vm.Id = vehicleType.Id;
            vm.VehicleTypeName = vehicleType.VehicleTypeName;

            return View(vm);
        }

        // POST: AdminArea/VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleType = await _context.VehicleTypes.SingleAsync(v => v.Id.Equals(id));
            if (await _context.Vehicles.AnyAsync(v => v.VehicleType.Id.Equals(vehicleType.Id)) 
                || await _context.Bookings.AnyAsync(b => b.VehicleTypeId.Equals(vehicleType.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(Guid id)
        {
            return _context.VehicleTypes.Any(e => e.Id == id);
        }
    }
}
