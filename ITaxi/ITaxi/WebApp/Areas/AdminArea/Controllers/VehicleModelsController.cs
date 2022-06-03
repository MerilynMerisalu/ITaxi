#nullable disable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class VehicleModelsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public VehicleModelsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/VehicleModels
        public async Task<IActionResult> Index()
        { 
            return View(await _uow.VehicleModels.GetAllAsync() );
        }

        // GET: AdminArea/VehicleModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleModelViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _uow.VehicleModels.FirstOrDefaultAsync(id.Value);
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
                await _uow.VehicleMarks.GetAllAsync(),
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
                _uow.VehicleModels.Add(vehicleModel);
                await _uow.SaveChangesAsync();
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

            var vehicleModel = await _uow.VehicleModels.FirstOrDefaultAsync(id.Value);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            vm.VehicleMarkId = vehicleModel.VehicleMark!.Id;
            vm.Id = vehicleModel.Id;
            vm.VehicleModelName = vehicleModel.VehicleModelName;
            vm.VehicleMarks = new SelectList(await _uow.VehicleMarks.GetAllAsync(),
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
            var vehicleModel = await _uow.VehicleModels.FirstOrDefaultAsync(id);
            if (vehicleModel != null && id != vehicleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vehicleModel != null)
                    {
                        vehicleModel.Id = id;
                        vehicleModel.VehicleMarkId = vm.VehicleMarkId;
                        vehicleModel.VehicleModelName = vm.VehicleModelName;
                        _uow.VehicleModels.Update(vehicleModel);
                        await _uow.SaveChangesAsync();
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (vehicleModel != null && !VehicleModelExists(vehicleModel.Id))
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

            var vehicleModel = await _uow.VehicleModels.FirstOrDefaultAsync(id.Value);
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
            var vehicleModel = await _uow.VehicleModels
                .FirstOrDefaultAsync(id);
            if (await _uow.Vehicles.AnyAsync(v => v.VehicleModelId.Equals(vehicleModel.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            if (vehicleModel != null)
            {
                _uow.VehicleModels.Remove(vehicleModel);
                await _uow.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(Guid id)
        {
            return _uow.VehicleModels.Exists(id);
        }
    }
}
