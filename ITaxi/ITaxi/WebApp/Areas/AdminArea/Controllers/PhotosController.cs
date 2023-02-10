
#nullable disable
using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
public class PhotosController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public PhotosController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Photos
    public async Task<IActionResult> Index()
    {
        var photos = await _uow.Photos.GetAllPhotosWithIncludesAsync();
        return View(photos);
    }

    // GET: AdminArea/Photos/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _uow.Photos.GetPhotoByIdAsync(id.Value);
        if (photo == null) return NotFound();

        photo.Id = vm.Id;
        photo.Title = vm.Title;
        photo.PhotoURL = vm.PhotoName;
        return View(vm);
    }

    // GET: AdminArea/Photos/Create
    public IActionResult Create()
    {
        var vm = new CreateEditPhotoViewModel();
        //ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email");
        return View(vm);
    }

    // POST: AdminArea/Photos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditPhotoViewModel vm, PhotoDTO photo)
    {
        if (ModelState.IsValid)
        {
            photo.Id = Guid.NewGuid();
            photo.Title = vm.Title;
            photo.PhotoURL = vm.PhotoName;
            _uow.Photos.Add(photo);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email", photo.AppUserId);
        return View(vm);
    }

    // GET: AdminArea/Photos/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditPhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _uow.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();
        //ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email", photo.AppUserId);
        return View(vm);
    }

    // POST: AdminArea/Photos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditPhotoViewModel vm)
    {
        var photo = await _uow.Photos.FirstOrDefaultAsync(id);
        if (photo != null && id != photo.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _uow.Photos.Update(photo!);
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

        //ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email", photo.AppUserId);
        return View(vm);
    }

    // GET: AdminArea/Photos/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _uow.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();

        return View(vm);
    }

    // POST: AdminArea/Photos/Delete/5
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
        return _uow.Photos.Exists(id);
    }
}


