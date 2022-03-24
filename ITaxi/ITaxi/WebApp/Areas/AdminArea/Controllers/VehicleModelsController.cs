#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class VehicleModelsController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleModelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/VehicleModels
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.VehicleModels
                .Include(v => v.VehicleMark);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/VehicleModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleModelViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels
                .Include(v => v.VehicleMark)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            vm.VehicleModelName = vehicleModel.VehicleModelName;
            vm.Id = vehicleModel.Id;
            vm.VehicleMarkName = vehicleModel.VehicleMark!.VehicleMarkName;
            return View(vm);
        }

        // GET: AdminArea/VehicleModels/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditVehicleModelViewModel();
            vm.VehicleMarks = new SelectList(
                await _context.VehicleMarks.OrderBy(v => v.VehicleMarkName)
                    .Select(v => new {v.Id, v.VehicleMarkName}).ToListAsync(),
                nameof(VehicleMark.Id), nameof(VehicleMark.VehicleMarkName));
            return View(vm);
        }

        // POST: AdminArea/VehicleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditVehicleModelViewModel vm,VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                vehicleModel.Id = Guid.NewGuid();
                vehicleModel.VehicleModelName = vm.VehicleModelName;
                vehicleModel.VehicleMarkId = vm.VehicleMarkId;
                _context.Add(vehicleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/VehicleModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditVehicleModelViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels
                .Include(v => v.VehicleMark).
                SingleOrDefaultAsync(v => v.Id.Equals(id));
            if (vehicleModel == null)
            {
                return NotFound();
            }

            vm.VehicleMarkId = vehicleModel.VehicleMark!.Id;
            vm.Id = vehicleModel.Id;
            vm.VehicleModelName = vehicleModel.VehicleModelName;
            vm.VehicleMarks = new SelectList(
                await _context.VehicleMarks.OrderBy(v => v.VehicleMarkName)
                    .Select(v => new {v.Id, v.VehicleMarkName}).ToListAsync(),
                nameof(VehicleMark.Id), nameof(VehicleMark.VehicleMarkName));
            return View(vm);
        }

        // POST: AdminArea/VehicleModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id)
        {
            var vm = new CreateEditVehicleModelViewModel();
            var vehicleModel = await _context.VehicleModels.Include(v => v.VehicleMark)
                .SingleAsync(v => v.Id.Equals(id));
            if (id != vehicleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleModel.Id = id;
                    vehicleModel.VehicleMarkId = vm.VehicleMarkId;
                    vehicleModel.VehicleModelName = vm.VehicleModelName;
                    _context.Update(vehicleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.Id))
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

        // GET: AdminArea/VehicleModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteVehicleModelViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels
                .Include(v => v.VehicleMark)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            vm.Id = vehicleModel.Id;
            vm.VehicleMarkName = vehicleModel.VehicleMark!.VehicleMarkName;
            vm.VehicleModelName = vehicleModel.VehicleModelName;
            return View(vm);
        }

        // POST: AdminArea/VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleModel = await _context.VehicleModels
                .SingleOrDefaultAsync(v => v.Id.Equals(id));
            if (await _context.Vehicles.AnyAsync(v => v.VehicleModelId.Equals(vehicleModel.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (vehicleModel != null) _context.VehicleModels.Remove(vehicleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(Guid id)
        {
            return _context.VehicleModels.Any(e => e.Id == id);
        }
    }
}
