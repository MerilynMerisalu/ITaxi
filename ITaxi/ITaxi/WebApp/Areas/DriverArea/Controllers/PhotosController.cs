using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.DriverArea.ViewModels;
using VehicleDTO = App.BLL.DTO.AdminArea.VehicleDTO;

namespace WebApp.Areas.DriverArea.Controllers;

[Area("DriverArea")]
[Authorize(Roles = "Admin, Driver")]
public class PhotosController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public PhotosController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: DriverArea/Photos
    public async Task<IActionResult> Index()
    {
        var res = await _uow.Photos.GetAllPhotosWithIncludesAsync();
        return View(res);
    }

    // GET: DriverArea/Photos/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();
        var photo = await _uow.Photos.GetPhotoByIdAsync(id.Value);
        if (photo == null) return NotFound();
        vm.Id = photo.Id;
        vm.Title = photo.Title;
        vm.PhotoURL = photo.PhotoURL;

        return View(vm);
    }

    // GET: DriverArea/Photos/Create
    public async Task<IActionResult> Create()
    {
        var userId = User.GettingUserId();
        var vm = new CreateEditPhotoViewModel();
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId),
            nameof(VehicleDTO.Id)
            , nameof(VehicleDTO.VehicleIdentifier));
        
        return View(vm);
    }

    // POST: DriverArea/Photos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditPhotoViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var photo = new PhotoDTO();
            photo.Id = Guid.NewGuid();
            photo.Title = vm.Title;
            photo.PhotoURL = vm.PhotoURL;
            photo.AppUserId = User.GettingUserId();
            _uow.Photos.Add(photo);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        return View(vm);
    }

    // GET: DriverArea/Photos/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditPhotoViewModel();
        var userId = User.GettingUserId();
        if (id == null) return NotFound();
        var photo = await _uow.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();
        vm.Id = photo.Id;
        vm.Title = photo.Title;
        vm.PhotoURL = photo.PhotoURL;
        vm.VehicleId = vm.VehicleId;
        vm.Vehicles = new SelectList(await _uow.Vehicles.GettingOrderedVehiclesAsync(userId),
            nameof(VehicleDTO.Id),
            nameof(VehicleDTO.VehicleIdentifier),
            nameof(vm.VehicleId));
        return View(vm);
    }

    // POST: DriverArea/Photos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
         CreateEditPhotoViewModel vm)
    {
        var userId = User.GettingUserId();
        var photo = await _uow.Photos.GetPhotoByIdAsync(id, userId);
        if (photo != null && id != photo.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (photo != null)
                {
                    photo.Title = vm.Title;
                    photo.AppUserId = userId;
                    photo.VehicleId = vm.VehicleId;
                    photo.CreatedBy = User.GettingUserEmail();
                    _uow.Photos.Update(photo);
                }

                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (photo != null && !PhotoExists(photo.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        
        return View(vm);
    }

    // GET: DriverArea/Photos/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _uow.Photos.GetPhotoByIdAsync(id.Value);
        if (photo == null) return NotFound();

        return View(vm);
    }

    // POST: DriverArea/Photos/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        
        var photo = await _uow.Photos.FirstOrDefaultAsync(id);
        if (photo != null) _uow.Photos.Remove(photo);

        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhotoExists(Guid id)
    {
        return (_uow.Photos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}