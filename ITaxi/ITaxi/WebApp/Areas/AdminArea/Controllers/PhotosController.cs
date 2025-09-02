#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using Base.Extensions;
using Base.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using PhotoDTO = App.BLL.DTO.AdminArea.PhotoDTO;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area photos controller
/// </summary>
[Area(nameof(AdminArea))]
public class PhotosController : Controller
{
    private readonly IAppBLL _appBLL;
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Admin area photos controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public PhotosController(IAppBLL appBLL, IWebHostEnvironment webHostEnvironment)
    {
        _appBLL = appBLL;
        _webHostEnvironment = webHostEnvironment;
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
    /*[HttpPost]
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
    }*/

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
    /*[HttpPost]
    [ValidateAntiForgeryToken]*/
    /*public async Task<IActionResult> Edit(Guid id, CreateEditPhotoViewModel vm)
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
    }*/

    // GET: AdminArea/Photos/Delete/5
    /// <summary>
    /// Admin area photos controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    /*public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeletePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();

        return View(vm);
    }*/

    // POST: AdminArea/Photos/Delete/5
    /// <summary>
    /// Admin area photos controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    /*[HttpPost]
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
    }*/

// GET: AdminArea/Vehicle/Gallery/5
    /// <summary>
    /// Admin area vehicle GET method gallery
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> VehiclesUpload(Guid? id)
    {
        if (id == null) return NotFound();

        var vm = new GalleryViewModel();

        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id.Value);
        if (vehicle == null) return NotFound();

        vm.VehicleIdentifier = vehicle.VehicleIdentifier;
        vm.Id = vehicle.Id;

        return View(vm);
    }



   
    [AcceptVerbs("Post")]
    public async Task<IActionResult> Upload([FromRoute] Guid id, List<IFormFile>? files)
    {
        string userRoleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, null, roleName:userRoleName);
        if (vehicle == null) return NotFound();
        
        var driverId = await _appBLL.Vehicles.GetDriverIdByVehicleIdAsync(vehicle.Id, roleName:userRoleName);
        if (driverId == null) return NotFound();
        
        if (files == null)
        {
            return Content(Common.FilesAreRequired);
        }
        int imagesAlreadyUploadedPerVehicle = await _appBLL.Photos.GetPhotoCountByVehicleIdAsync(
            id, null, userRoleName, true);
        
        bool result = _appBLL.Photos.AlreadyHasACertainNumberOfImages(files:files, numberOfImages: imagesAlreadyUploadedPerVehicle);
            
        if (result)
        {
            return Content(string.Format(Common.NumberOfImagesErrorMessage, "four"));
        }
        
        result = _appBLL.Photos.AreAllFilesCorrect(files);
        if (!result) 
        {
            return Content(string.Format(Common.FilesAreNotCorrect, "1", "b", "5", "MB"));
        }

        int fileTitleMaxLength = 255;
        foreach (var file in files)
        {
            string fileName = _appBLL.Photos.FileNameFormat(fileName: file.FileName,
                maxLength: fileTitleMaxLength);
            string fileExtension = Path.GetExtension(file.FileName);
            string? directoryId = await _appBLL.Photos.GetDirectoryIdByVehicleIdAsStringAsync(vehicle.Id,
                null, userRoleName); 
            string[]? directoryNames = { "Vehicles" };
            
           string fullUploadFolderPath = _appBLL.Photos.GetDirectoryPath(_webHostEnvironment.WebRootPath, directoryNames!);
           if (fullUploadFolderPath == null)
           {
               throw new ArgumentNullException(fullUploadFolderPath);
           }
            if (directoryId == null)
            {
               directoryId = Guid.NewGuid().ToString();
              fullUploadFolderPath = Path.Combine(fullUploadFolderPath, directoryId);
              Directory.CreateDirectory(fullUploadFolderPath);
            }
            else
            {
                fullUploadFolderPath = Path.Combine(fullUploadFolderPath, directoryId);
            }
            
            
            string fileNameOnDisk =  _appBLL.Photos.GetFileNameForDirectory(fullUploadFolderPath, fileExtension);
            
            string relativeFilePath = Path.GetRelativePath(_webHostEnvironment.WebRootPath, 
                Path.Combine(fullUploadFolderPath, fileNameOnDisk));
            if (_appBLL.Photos.DoesFileExist(Path.Combine(fullUploadFolderPath, fileNameOnDisk)))
            {
                return Content(string.Format(Common.FileExists, fileNameOnDisk));
            }
            
            result = await _appBLL.Photos.UploadImagesAsync(fullUploadFolderPath, fileNameOnDisk, file);
            if (!result)
                return Content(Common.UploadFailed);

            var photo = new PhotoDTO()
            {
                Id = Guid.NewGuid(),
                Title = fileName,
                DriverId = driverId,
                VehicleId = vehicle.Id,
                DirectoryTitleId = directoryId,
                FileNameInDirectory = fileNameOnDisk,
                PhotoURL = relativeFilePath,
                CreatedBy = User.GettingUserEmail(),
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = User.GettingUserEmail(),
                UpdatedAt = DateTime.UtcNow,
            };
            _appBLL.Photos.Add(photo);
            await _appBLL.SaveChangesAsync();
            
            
        }
        
        return RedirectToAction("Gallery", "Vehicles" );
    }
}



