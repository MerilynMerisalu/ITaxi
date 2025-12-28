#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using Base.Extensions;
using Base.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using WebApp.Helpers;
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

        var res = await _appBLL.Photos.GetAllPhotosWithIncludesAsync(roleName: null);

        return View(res);
    }

    // GET: AdminArea/Photos/Details/5
    /// <summary>
    /// Admin area photos controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> VehiclePhotoDetails(Guid? id)
    {
        var vm = new DetailsDeleteVehiclePhotoViewModel();
        if (id == null) return NotFound();

        var photo = await _appBLL.Photos.GetPhotoByIdAsync(id.Value);
        if (photo == null) return NotFound();
        vm.Id = photo.Id;
        var title = FileHelper.ReplaceUnderscoreWithSpaceInFileName(photo.Title);
        vm.Title = title;
        vm.FileName = FileHelper.ReplaceUnderscoreWithSpaceInFileName(photo.FileName);
        vm.PhotoURL = photo.PhotoURL!;
        vm.PhotoFullPath = photo.PhotoFullPath!;
        vm.ThumbnailRelativePath = photo.ThumbnailRelativePath!;
        vm.ThumbnailFullPath = photo.ThumbnailFullPath!;
        vm.DirectoryTitleId = photo.DirectoryTitleId;
        vm.FileNameInDirectory = photo.FileNameInDirectory;
        if (photo.VehicleId.HasValue)
        {
            var isVehicle = await _appBLL.Photos.IsPhotoOfVehicleAsync(photoId: photo.Id, vehicleId: photo.VehicleId.Value);

            if (isVehicle)
            {
                vm.VehicleId = photo.VehicleId.Value;
                var vehicleIdentifier = await _appBLL.Photos.GetVehicleIdentifierAsync(photoId: photo.Id, vehicleId: photo.VehicleId.Value);
                var driver = await _appBLL.Vehicles.GetVehicleDriverByVehicleIdAsync(vehicleId: photo.VehicleId.Value);
                vm.IsVehicle = isVehicle;
                vm.Vehicle = vehicleIdentifier;
                vm.VehicleDriver = driver;
            }
        }

        if (photo.AdminId.HasValue)

            vm.CreatedBy = photo.CreatedBy;
        vm.CreatedAt = photo.CreatedAt;
        vm.UpdatedBy = photo.UpdatedBy;
        vm.UpdatedAt = photo.UpdatedAt;

        return View(vm);
    }




    // GET: AdminArea/Photos/VehiclePhotoDelete/5
    /// <summary>
    /// Admin area photos controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> VehiclePhotoDelete(Guid? id, Guid vehicleId)
    {
        var vm = new DetailsDeleteVehiclePhotoViewModel();
        if (id == null) return NotFound();
        var roleName = User.GettingUserRoleName();
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id.Value);
        if (photo == null) return NotFound();

        vm.Id = photo.Id;
        var title = FileHelper.ReplaceUnderscoreWithSpaceInFileName(photo.Title);
        vm.Title = title;
        vm.FileName = FileHelper.ReplaceUnderscoreWithSpaceInFileName(photo.FileName);
        vm.PhotoURL = photo.PhotoURL!;
        vm.PhotoFullPath = photo.PhotoFullPath!;
        vm.ThumbnailRelativePath = photo.ThumbnailRelativePath;
        vm.ThumbnailFullPath = photo.ThumbnailFullPath!;
        vm.DirectoryTitleId = photo.DirectoryTitleId;
        vm.FileNameInDirectory = photo.FileNameInDirectory;
        if (photo.VehicleId.HasValue)
        {
            var isVehicle = await _appBLL.Photos.IsPhotoOfVehicleAsync(photoId: photo.Id, vehicleId: photo.VehicleId.Value);

            if (isVehicle)
            {
                vm.IsVehicle = isVehicle;
                var vehicleTypeName = await _appBLL.Vehicles.GetVehicleTypeNameByVehicleIdAsync(vehicleId: photo.VehicleId.Value, userId: null, roleName: null);
                var vehicleMark = await _appBLL.Vehicles.GetVehicleMarkNameByVehicleIdAsync(vehicleId: photo.VehicleId.Value, userId: null, roleName: null);
                var vehicleModel = await _appBLL.Vehicles.GetVehicleModelNameByVehicleIdAsync(vehicleId: photo.VehicleId.Value, userId: null, roleName: null);
                var vehiclePlateNumber = await _appBLL.Vehicles.GetVehiclePlateNumberByVehicleIdAsync(vehicleId: photo.VehicleId.Value, userId: null, roleName: null);
                var driverId = await _appBLL.Vehicles.GetDriverIdByVehicleIdAsync(vehicleId: photo.VehicleId.Value);
                var driver = await _appBLL.Vehicles.GetVehicleDriverByVehicleIdAsync(vehicleId: photo.VehicleId.Value);
                vm.VehicleId = photo.VehicleId.Value;
                vm.IsVehicle = isVehicle;
                vm.VehicleDriver = driver;
                var vehicleIdentifier = $"{vehicleTypeName} {vehicleMark} {vehicleModel} {vehiclePlateNumber}";
                vm.Vehicle = vehicleIdentifier;


            }
        }


        vm.CreatedBy = photo.CreatedBy;
        vm.CreatedAt = photo.CreatedAt;
        vm.UpdatedBy = photo.UpdatedBy;
        vm.UpdatedAt = photo.UpdatedAt;


        return View(vm);
    }

    // POST: AdminArea/Photos/Delete/5
    /// <summary>
    /// Admin area photos controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vehicleId">Vehicle to which the photo belongs</param>
    /// <returns>Redirect to vehicle index</returns>
    [HttpPost]
    [ActionName(nameof(VehiclePhotoDelete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, Guid vehicleId)
    {
        var userRole = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(vehicleId, null, roleName: userRole);
        if (vehicle == null) return NotFound();
        var photo = await _appBLL.Photos.GetPhotoByIdAsync(id, roleName: userRole);
        if (photo == null) return NotFound();
        if (photo.VehicleId != vehicleId) return Forbid();
        var vehicleImageFolderId = await _appBLL.Photos.GetDirectoryIdByVehicleIdAsStringAsync(vehicleId, roleName: userRole);
        if (vehicleImageFolderId == null) return NotFound();
        var imageThumbnailFullPath = photo.ThumbnailFullPath;
        var fullImagePath = photo.PhotoFullPath;
        if (imageThumbnailFullPath != null)
        {
            FileHelper.DeleteFile(imageThumbnailFullPath);
        }
        if (fullImagePath != null)
        {
            FileHelper.DeleteFile(fullImagePath);
        }

        photo.IsDeleted = true;
        photo.DeletedBy = User.GettingUserEmail();
        photo.DeletedAt = DateTime.UtcNow;

        await _appBLL.Photos.RemoveAsync(photo.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction("Gallery", controllerName: "Vehicles", routeValues: new { vehicleId = photo.VehicleId });
    }

    private bool PhotoExists(Guid id)
    {
        return _appBLL.Photos.Exists(id);
    }

    // GET: AdminArea/Vehicle/Gallery/5
    /// <summary>
    /// Admin area vehicle GET method gallery
    /// </summary>
    /// <param name="vehicleId">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> VehicleImagesUpload(Guid? vehicleId)
    {
        if (vehicleId == null) return NotFound();
        var userRole = User.GettingUserRoleName();

        var vm = new VehicleImagesUploadViewModel();

        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id: vehicleId.Value);
        if (vehicle == null) return NotFound();

        vm.VehicleIdentifier = vehicle.VehicleIdentifier;
        vm.VehicleId = vehicle.Id;

        return View(vm);
    }

    [AcceptVerbs("Post")]
    public async Task<IActionResult> VehicleImagesUpload(Guid vehicleId, IFormFile? photo1,
        IFormFile? photo2,
        IFormFile? photo3,
        IFormFile? photo4)
    {
        List<IFormFile> files = new List<IFormFile> { photo1, photo2, photo3, photo4 };

        string userRoleName = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id: vehicleId, null, roleName: userRoleName);
        if (vehicle == null) return NotFound();

        var driverId = await _appBLL.Vehicles.GetDriverIdByVehicleIdAsync(vehicle.Id, roleName: userRoleName);
        if (driverId == null) return NotFound();

        if (files == null || !files.Any())
            return Content(Common.FilesAreRequired);

        int imagesAlreadyUploaded = await _appBLL.Photos.GetPhotoCountByVehicleIdAsync(vehicleId, null, userRoleName, true);
        if (_appBLL.Photos.AlreadyHasACertainNumberOfImages(imagesAlreadyUploaded, files: files))
            return Content(string.Format(Common.NumberOfImagesErrorMessage, "4"));

        if (!_appBLL.Photos.AreAllFilesCorrect(files))
            return Content(string.Format(Common.FilesAreNotCorrect, "1", "b", "5", "MB"));


        string? directoryId = await _appBLL.Photos.GetDirectoryIdByVehicleIdAsStringAsync(vehicle.Id, null, userRoleName);
        string[] directoryNames = { "Vehicles" };
        string uploadFolderPath = _appBLL.Photos.GetDirectoryPath(_webHostEnvironment.WebRootPath, directoryNames);
        const string THUMBNAILFOLDERNAME = "Thumbnails";
        string thumbnailFolderPath = string.Empty;
        if (string.IsNullOrEmpty(directoryId))
        {
            directoryId = Guid.NewGuid().ToString();
            uploadFolderPath = Path.Combine(uploadFolderPath, directoryId);
            if (!_appBLL.Photos.DoesDirectoryExist(uploadFolderPath))
                _appBLL.Photos.CreateDirectory(uploadFolderPath);
            thumbnailFolderPath = Path.Combine(uploadFolderPath, THUMBNAILFOLDERNAME);
            if (!_appBLL.Photos.DoesDirectoryExist(thumbnailFolderPath))
                _appBLL.Photos.CreateDirectory(thumbnailFolderPath);
        }
        else
        {
            uploadFolderPath = Path.Combine(uploadFolderPath, directoryId);
            thumbnailFolderPath = Path.Combine(uploadFolderPath, THUMBNAILFOLDERNAME);
        }


        foreach (var file in files)
        {
            if (file == null)
            {
                continue;
            }

            int fileNameMaximumLength = 255;
            string fileName = _appBLL.Photos.FileNameFormat(file.FileName, fileNameMaximumLength);
            string title = _appBLL.Photos.FileNameFormat(file.FileName, fileNameMaximumLength);
            title = FileHelper.ReplaceUnderscoreWithSpaceInFileName(title);
            title = FileHelper.RemoveFileExtensionFromTitle(title);
            string fileExtension = Path.GetExtension(file.FileName);
            string fileNameOnDisk = _appBLL.Photos.GetFileNameForDirectory(uploadFolderPath, fileExtension);
            string relativeFilePath = Path.GetRelativePath(_webHostEnvironment.WebRootPath,
                Path.Combine(uploadFolderPath, fileNameOnDisk));
            relativeFilePath = FileHelper.GetImageRelativePath(relativeFilePath);
            string fullFilePath = FileHelper.GetFileFullPath(uploadFolderPath, fileNameOnDisk);
            if (_appBLL.Photos.DoesFileExist(Path.Combine(uploadFolderPath, fileNameOnDisk)))
                return Content(string.Format(Common.FileExists, fileNameOnDisk));

            bool uploadResult = await _appBLL.Photos.UploadImagesAsync(uploadFolderPath, fileNameOnDisk, file);
            if (!uploadResult)
                return Content(Common.UploadFailed);
            var thumbnailFilePath = await _appBLL.Photos.CreateThumbnailAsync(fullFilePath, fileName: fileName, fileExtension, thumbnailFolderPath);
            var thumbnailRelativePath = FileHelper.GetImageRelativePath(Path.GetRelativePath(_webHostEnvironment.WebRootPath, thumbnailFilePath));

            var photo = new PhotoDTO()
            {
                Id = Guid.NewGuid(),
                Title = title,
                FileName = fileName,
                DriverId = driverId,
                VehicleId = vehicle.Id,
                DirectoryTitleId = directoryId,
                PhotoFullPath = fullFilePath,
                ThumbnailFullPath = thumbnailFilePath,
                FileNameInDirectory = fileNameOnDisk,
                PhotoURL = relativeFilePath,
                ThumbnailRelativePath = thumbnailRelativePath,
                CreatedBy = User.GettingUserEmail(),
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = User.GettingUserEmail(),
                UpdatedAt = DateTime.UtcNow,
            };
            _appBLL.Photos.Add(photo);
        }

        await _appBLL.SaveChangesAsync();
        return RedirectToAction("ChooseView", new { vehicleId = vehicle.Id });
    }




    [HttpPost]
    [ActionName("UpdateVehicleImageAsync")]
    public async Task<IActionResult> UpdateVehicleImageAsync(Guid vehicleId, Guid vehicleImageId, IFormFile file)
    {
        var userRole = User.GettingUserRoleName();
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(vehicleId, null, roleName: userRole);
        if (vehicle == null) return NotFound();
        var photo = await _appBLL.Photos.GetPhotoByIdAsync(vehicleImageId, roleName: userRole);
        if (photo == null) return NotFound();
        if (photo.VehicleId != vehicle.Id) return Forbid();
        var vehicleImageFolderId = await _appBLL.Photos.GetDirectoryIdByVehicleIdAsStringAsync(vehicleId, roleName: userRole);
        if (vehicleImageFolderId == null) return NotFound();
        if (file == null)
        {
            return Content(Common.FilesAreRequired);
        }
        List<IFormFile?>? files = new List<IFormFile?>();
        files.Add(file);

        var imageThumbnailFullPath = photo.ThumbnailFullPath;
        var fullImagePath = photo.PhotoFullPath;
        if (imageThumbnailFullPath != null)
        {
            FileHelper.DeleteFile(imageThumbnailFullPath);
        }
        if (fullImagePath != null)
        {
            FileHelper.DeleteFile(fullImagePath);
        }
        photo.IsDeleted = true;
        photo.DeletedBy = User.GettingUserEmail();
        photo.DeletedAt = DateTime.UtcNow;

        await _appBLL.Photos.RemoveAsync(photo.Id);
        await _appBLL.SaveChangesAsync();

        int imagesAlreadyUploaded = await _appBLL.Photos.GetPhotoCountByVehicleIdAsync(vehicleId, null, userRole, true);
        if (_appBLL.Photos.AlreadyHasACertainNumberOfImages(imagesAlreadyUploaded, files: files))
            return Content(string.Format(Common.NumberOfImagesErrorMessage, "4"));

        if (!_appBLL.Photos.AreAllFilesCorrect(files))
            return Content(string.Format(Common.FilesAreNotCorrect, "1", "b", "5", "MB"));
        string[] directoryNames = { "Vehicles" };
        string uploadFolderPath = _appBLL.Photos.GetDirectoryPath(_webHostEnvironment.WebRootPath, directoryNames, vehicleImageFolderId);
        const string THUMBNAILFOLDERNAME = "Thumbnails";
        int fileNameMaximumLength = 255;
        string fileName = _appBLL.Photos.FileNameFormat(file.FileName, fileNameMaximumLength);
        string fileExtension = Path.GetExtension(file.FileName);
        string fileNameOnDisk = _appBLL.Photos.GetFileNameForDirectory(uploadFolderPath, fileExtension);
        string relativeFilePath = Path.GetRelativePath(_webHostEnvironment.WebRootPath,
            Path.Combine(uploadFolderPath, fileNameOnDisk));
        relativeFilePath = FileHelper.GetImageRelativePath(relativeFilePath);
        string fullFilePath = FileHelper.GetFileFullPath(uploadFolderPath, fileNameOnDisk);
        if (_appBLL.Photos.DoesFileExist(Path.Combine(uploadFolderPath, fileNameOnDisk)))
            return Content(string.Format(Common.FileExists, fileNameOnDisk));
        bool uploadResult = await _appBLL.Photos.UploadImagesAsync(uploadFolderPath, fileNameOnDisk, file);
        if (!uploadResult)
            return Content(Common.UploadFailed);
        var thumbnailFolderPath = Path.Combine(uploadFolderPath, THUMBNAILFOLDERNAME);
        var thumbnailFilePath = await _appBLL.Photos.CreateThumbnailAsync(fullFilePath, fileName: fileName, fileExtension, thumbnailFolderPath);
        var thumbnailRelativePath = FileHelper.GetImageRelativePath(Path.GetRelativePath(_webHostEnvironment.WebRootPath, thumbnailFilePath));
        string title = FileHelper.RemoveFileExtensionFromTitle(fileName);
        title = FileHelper.ReplaceUnderscoreWithSpaceInFileName(title);
        fileName = FileHelper.ReplaceUnderscoreWithSpaceInFileName(fileName);
        var replacementPhoto = new PhotoDTO()
        {
            Id = Guid.NewGuid(),
            Title = title,
            FileName = fileName,
            VehicleId = vehicle.Id,
            DirectoryTitleId = vehicleImageFolderId,
            PhotoFullPath = fullFilePath,
            PhotoURL = relativeFilePath,
            FileNameInDirectory = fileNameOnDisk,
            ThumbnailFullPath = thumbnailFilePath,
            ThumbnailRelativePath = thumbnailRelativePath,
            CreatedBy = User.GettingUserEmail(),
            CreatedAt = DateTime.UtcNow,
            UpdatedBy = User.GettingUserEmail(),
            UpdatedAt = DateTime.UtcNow,
        };
        _appBLL.Photos.Add(replacementPhoto);
        await _appBLL.SaveChangesAsync();

        return RedirectToAction("ChooseView", new { vehicleId = vehicle.Id });
    }
    /// <summary>
    /// Choosing a right view
    /// </summary>
    /// <param name="vehicleId">VehicleId</param>
    /// <returns></returns>
    public async Task<IActionResult> ChooseView(Guid? vehicleId = null)
    {
        if (!vehicleId.HasValue)
        {
            return View();
        }

        var roleName = User.GettingUserRoleName();

        var vehicle = await _appBLL.Vehicles
            .GettingVehicleWithIncludesByIdAsync(vehicleId.Value, roleName: null);

        if (vehicle == null)
        {
            return NotFound();
        }

        int numberOfPhotos = await _appBLL.Photos
            .GetPhotoCountByVehicleIdAsync(vehicle.Id);

        if (numberOfPhotos == 0)
        {
            return RedirectToAction(
                "VehicleImagesUpload",
                new { vehicleId = vehicle.Id }
            );
        }

        return RedirectToAction(
            "Gallery",
            "Vehicles",
            new { vehicleId = vehicle.Id }
        );
    }



    [HttpPost("{vehicleId:guid}")]
    
    public async Task<IActionResult> DeleteAllVehiclePhotos([FromRoute]Guid vehicleId)
    {
        var vehicle = await _appBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id: vehicleId);
        if (vehicle == null) return NotFound();

        return RedirectToAction("Gallery", "Vehicles", new { vehicleId = vehicle.Id });
    }
}








