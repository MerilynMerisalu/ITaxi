#nullable disable
using App.Contracts.DAL;
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
        private readonly IAppUnitOfWork _uow;

        public VehicleTypesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/VehicleTypes
        public async Task<IActionResult> Index()
        {
            return View(await _uow.VehicleTypes.GetAllAsync());
        }

        // GET: AdminArea/VehicleTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleTypeViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id.Value);
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
                _uow.VehicleTypes.Add(vehicleType);
                await _uow.SaveChangesAsync();
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

            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id.Value);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditVehicleTypeViewModel vm)
        {
            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id);
            
            if (vehicleType != null && id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vehicleType != null)
                    {
                        vehicleType.Id = vm.Id;
                        vehicleType.VehicleTypeName = vm.VehicleTypeName;
                        _uow.VehicleTypes.Update(vehicleType);
                        await _uow.SaveChangesAsync();
                    }

                    
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

            var vehicleType = await _uow.VehicleTypes
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
            var vehicleType = await _uow.VehicleTypes.FirstOrDefaultAsync(id);
            if (await _uow.Vehicles.AnyAsync(v => v.VehicleType.Id.Equals(vehicleType.Id)) 
                || await _uow.Bookings.AnyAsync(b => b.VehicleTypeId.Equals(vehicleType.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            if (vehicleType != null)
            {
                _uow.VehicleTypes.Remove(vehicleType);
                await _uow.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(Guid id)
        {
            return _uow.VehicleTypes.Exists(id);
        }
    }
}
