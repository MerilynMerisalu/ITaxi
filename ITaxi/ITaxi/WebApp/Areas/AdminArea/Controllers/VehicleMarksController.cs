#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    [Authorize(Roles = nameof(Admin))]
    public class VehicleMarksController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public VehicleMarksController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/VehicleMarks
        public async Task<IActionResult> Index()
        {
            var res = await _uow.VehicleMarks.GetAllVehicleMarkOrderedAsync();
            foreach (var vehicleMark in res)
            {
                vehicleMark.CreatedAt = vehicleMark.CreatedAt.ToLocalTime();
                vehicleMark.UpdatedAt = vehicleMark.UpdatedAt.ToLocalTime();
            }

            return View(res);
        }

        // GET: AdminArea/VehicleMarks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteVehicleMarkViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMark = await _uow.VehicleMarks
                .FirstOrDefaultAsync(id.Value);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            vm.VehicleMarkName = vehicleMark.VehicleMarkName;
            vm.Id = vehicleMark.Id;
            vm.CreatedBy = vehicleMark.CreatedBy!;
            vm.CreatedAt = vehicleMark.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = vehicleMark.UpdatedBy!;
            vm.UpdatedAt = vehicleMark.UpdatedAt.ToLocalTime().ToString("G");

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
                _uow.VehicleMarks.Add(vehicleMark);
                await _uow.SaveChangesAsync();
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

            var vehicleMark = await _uow.VehicleMarks.FirstOrDefaultAsync(id.Value);
            if (vehicleMark?.VehicleMarkName != null)
            {
                vm.VehicleMarkName = vehicleMark.VehicleMarkName;
            } 
                

            return View(vm);
        }

        // POST: AdminArea/VehicleMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditVehicleMarkViewModel vm)
        {
            
            var vehicleMark = await _uow.VehicleMarks
                .FirstOrDefaultAsync(id);
            if (vehicleMark != null && id != vehicleMark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vehicleMark != null)
                    {
                        vehicleMark.Id = id;
                        vehicleMark.VehicleMarkName = vm.VehicleMarkName;
                        _uow.VehicleMarks.Update(vehicleMark);
                        await _uow.SaveChangesAsync();
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (vehicleMark != null && !VehicleMarkExists(vehicleMark.Id))
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

            var vehicleMark = await _uow.VehicleMarks
                .FirstOrDefaultAsync(id.Value);
            if (vehicleMark == null)
            {
                return NotFound();
            }

            vm.Id = vehicleMark.Id;
            vm.VehicleMarkName = vehicleMark.VehicleMarkName;
            vm.CreatedBy = vehicleMark.CreatedBy!;
            vm.CreatedAt = vehicleMark.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = vehicleMark.UpdatedBy!;
            vm.UpdatedAt = vehicleMark.UpdatedAt.ToLocalTime().ToString("G");


            return View(vm);
        }

        // POST: AdminArea/VehicleMarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vehicleMark = await _uow.VehicleMarks
                .FirstOrDefaultAsync(id);
            if (await _uow.VehicleModels.AnyAsync(v => vehicleMark != null && v != null && v.VehicleMarkId.Equals(vehicleMark.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            if (vehicleMark != null)
            {
                _uow.VehicleMarks.Remove(vehicleMark);
                await _uow.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMarkExists(Guid id)
        {
            return _uow.VehicleMarks.Exists(id);
        }
    }
}
