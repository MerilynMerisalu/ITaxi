#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class VehicleMarksController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleMarksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/VehicleMarks
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleMarks.ToListAsync());
        }

        // GET: AdminArea/VehicleMarks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleMarkViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            vm.VehicleMarkName = vehicleMark.VehicleMarkName;
            vm.Id = vehicleMark.Id;

            return View(vm);
        }

        // GET: AdminArea/VehicleMarks/Create
        public IActionResult Create()
        {
            var vm = new CreateEditVehicleMarkViewModel();
            return View(vm);
        }

        // POST: AdminArea/VehicleMarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditVehicleMarkViewModel vm, VehicleMark vehicleMark)
        {
            if (ModelState.IsValid)
            {
                vehicleMark.Id = Guid.NewGuid();
                vehicleMark.VehicleMarkName = vm.VehicleMarkName;
                _context.Add(vehicleMark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/VehicleMarks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditVehicleMarkViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks.SingleAsync(v => v.Id.Equals(id));
            vm.VehicleMarkName = vehicleMark.VehicleMarkName;
            
            return View(vm);
        }

        // POST: AdminArea/VehicleMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditVehicleMarkViewModel vm)
        {
            
            var vehicleMark = await _context.VehicleMarks
                .SingleAsync(v => v.Id.Equals(id));
            if (id != vehicleMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleMark.Id = id;
                    vehicleMark.VehicleMarkName = vm.VehicleMarkName;
                    _context.Update(vehicleMark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMarkExists(vehicleMark.Id))
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

        // GET: AdminArea/VehicleMarks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteVehicleMarkViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _context.VehicleMarks
                .SingleOrDefaultAsync(m => m.Id == id);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            vm.Id = vehicleMark.Id;
            vm.VehicleMarkName = vehicleMark.VehicleMarkName;

            return View(vm);
        }

        // POST: AdminArea/VehicleMarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleMark = await _context.VehicleMarks
                .SingleOrDefaultAsync(v => v.Id.Equals(id));
            if (await _context.VehicleModels.AnyAsync(v => v.VehicleMarkId.Equals(vehicleMark.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (vehicleMark != null) _context.VehicleMarks.Remove(vehicleMark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMarkExists(Guid id)
        {
            return _context.VehicleMarks.Any(e => e.Id == id);
        }
    }
}
