#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area photos controller
/// </summary>
[Area(nameof(AdminArea))]
public class PhotosController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area photos controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public PhotosController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Photos
    /// <summary>
    /// Admin area photos controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Photos.GetAllPhotosWithIncludesAsync();
        return View(res);
    }

    // GET: AdminArea/Photos/Details/5
    /// <summary>
    /// Admin area photos controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _appBLL.Photos.GetPhotoByIdAsync(id.Value);
        if (photo == null) return NotFound();

        photo.Id = vm.Id;
        photo.Title = vm.Title;
        photo.PhotoURL = vm.PhotoName;
        return View(vm);
    }

    // GET: AdminArea/Photos/Create
    /// <summary>
    /// Admin area photos controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditPhotoViewModel();
        return View(vm);
    }

    // POST: AdminArea/Photos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area photos controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="photo">Photo</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditPhotoViewModel vm, PhotoDTO photo)
    {
        if (ModelState.IsValid)
        {
            photo.Id = Guid.NewGuid();
            photo.Title = vm.Title;
            photo.PhotoURL = vm.PhotoName;
            _appBLL.Photos.Add(photo);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View(vm);
    }

    // GET: AdminArea/Photos/Edit/5
    /// <summary>
    /// Admin area photos controller GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditPhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();
        
        return View(vm);
    }

    // POST: AdminArea/Photos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area photos controller POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditPhotoViewModel vm)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);
        if (photo != null && id != photo.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (photo != null) _appBLL.Photos.Update(photo);
                await _appBLL.SaveChangesAsync();
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

    // GET: AdminArea/Photos/Delete/5
    /// <summary>
    /// Admin area photos controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();

        return View(vm);
    }

    // POST: AdminArea/Photos/Delete/5
    /// <summary>
    /// Admin area photos controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);
        if (photo != null) _appBLL.Photos.Remove(photo);

        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhotoExists(Guid id)
    {
        return _appBLL.Photos.Exists(id);
    }
}


