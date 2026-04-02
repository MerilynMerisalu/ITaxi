using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts.Mappers;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Globalization;
using System.IO;
using System.Text;

namespace App.BLL.Services;

public class PhotoService : BaseEntityService<App.BLL.DTO.AdminArea.PhotoDTO,
    App.DAL.DTO.AdminArea.PhotoDTO, IPhotoRepository>, IPhotoService
{
    public PhotoService(IPhotoRepository repository, IMapper<PhotoDTO, DAL.DTO.AdminArea.PhotoDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<PhotoDTO?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository
                .GetAllPhotosWithIncludesAsync(userId, roleName, noTracking))
            .Select(e => Mapper.Map(e)).OfType<PhotoDTO>();
    }

    public IEnumerable<PhotoDTO?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GetAllPhotosWithIncludes(userId, roleName, noTracking).Select(e => Mapper.Map(e))
            .Where(dto => dto != null).Select(dto => dto!).OfType<PhotoDTO>();
    }

    public async Task<PhotoDTO?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GetPhotoByIdAsync(id, userId, roleName, noTracking));
    }

    public PhotoDTO? GetPhotoById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(Repository.GetPhotoById(id, userId, roleName, noTracking));
    }

    public async Task<int> GetPhotoCountByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool isCountingDeleted = false)
    {
        return await Repository.GetPhotoCountByVehicleIdAsync(vehicleId, userId, roleName, noTracking, isCountingDeleted);
    }

    public int GetPhotoCountByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool isCountingDeleted = false)
    {
        return Repository.GetPhotoCountByVehicleId(vehicleId, userId, roleName, noTracking, isCountingDeleted);
    }

    public bool AlreadyHasACertainNumberOfImages(int numberOfImages, int? numberOfImagesAllowed = 4, List<IFormFile>? files = null)
    {
        int minimumNumberOfImagesAllowed = 1;
        int numberOfFiles = 0;
        if (files == null) throw new ArgumentNullException(nameof(files));
        foreach (var file in files)
        {
            if (file == null)
            {
                continue;
            }
            numberOfFiles += 1;

            if (numberOfImagesAllowed < minimumNumberOfImagesAllowed)
                throw new ArgumentOutOfRangeException(nameof(numberOfImagesAllowed));
            else if ((numberOfFiles + numberOfImages) > numberOfImagesAllowed)
                return true;
        }

        return false;
    }

    public bool AreAllFilesCorrect(List<IFormFile> files)
    {
        foreach (IFormFile file in files)

            if (file == null)
            {
                continue;
            }
            else
            {
                if (file.Length <= 0 || file.Length > 5000000)
                    return false;
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!fileExtension.Equals(".png") && !fileExtension.Equals(".jpg"))
                {
                    return false;
                }

                try
                {
                    using var imageStream = file.OpenReadStream();
                    var image = Image.Identify(imageStream);
                }
                catch(Exception)
                {
                    return false;
                }

            }
        return true;
    }





    public bool DoesFileExist(string fullFilePath)
    {
        if (!File.Exists(fullFilePath))
            return false;
        return true;
    }

    public string? CreateDirectoryPath(string startOfDirectoryPath, string[]? middleParts, string? directoryName = null)
    {
        var directoryFullPath = Path.Combine(startOfDirectoryPath, "Images\\");
        if (string.IsNullOrWhiteSpace(startOfDirectoryPath))
            throw new ArgumentNullException(nameof(startOfDirectoryPath));
        if (middleParts!.Any() || middleParts != null)
        {
            foreach (var mp in middleParts!)
            {
                string middlePart = string.Concat(mp.Split(Path.GetInvalidPathChars())).Trim();
                directoryFullPath += Path.Combine(middlePart);
            }

        }

        if (directoryName != null)
        {
            return directoryFullPath = Path.Combine(directoryFullPath, directoryName);
        }

        else
        {
            return directoryFullPath;
        }

    }

    public bool DoesDirectoryExist(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return false;
        }
        return true;
    }

    public void CreateDirectory(string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);
    }

    public async Task<string?> GetDirectoryIdByVehicleIdAsStringAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
        return await Repository.GetDirectoryIdByVehicleIdAsStringAsync(vehicleId, userId, roleName, noTracking, noIncludes);
    }

    public string? GetDirectoryIdByVehicleIdAsString(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
        return Repository.GetDirectoryIdByVehicleIdAsString(vehicleId, userId, roleName, noTracking, noIncludes);
    }

    //public async Task<Image> UploadImagesAsync(string fullFilePath, string fileNameOnDisk, IFormFile file)
    //{
    //    fullFilePath = Path.Combine(fullFilePath, fileNameOnDisk);

    //    try
    //    {
    //        using var uploadStream = file.OpenReadStream();
    //        var image = await Image.LoadAsync(uploadStream);
    //        await image.SaveAsync(fullFilePath);
    //        return image;
    //    }
    //    catch (Exception ex) when (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException
    //        || ex is IOException)
    //    {

    //        return false;

    //    }
    //    return true;
    //}

    public string FileNameFormat(string fileName, int maxLength)
    {
        if (fileName.Length > maxLength)
            fileName = fileName.Substring(0, maxLength) + "...";
        char firstChar = fileName[0];
        if (Char.IsLower(firstChar) == true)
        {
            fileName = fileName.Replace(firstChar.ToString(), firstChar.ToString().ToUpperInvariant());
        }
        return fileName.Replace(" ", "_").Trim();
    }

    public string GetFileNameForDirectory(string fullUploadDirectoryPath, string fileExtension, int? numberOfPhotos = 4, string? entityName = "vehicle")
    {
        const string filenameBeginning = "Foto_";
        fullUploadDirectoryPath = fullUploadDirectoryPath.TrimEnd(Path.DirectorySeparatorChar);

        for (int i = 1; i <= numberOfPhotos; i++)
        {
            string fileNameInDirectory = $"{filenameBeginning}{i}{fileExtension}";
            string fullPath = Path.Combine(fullUploadDirectoryPath, fileNameInDirectory);

            if (!DoesFileExist(fullPath))
            {
                return fileNameInDirectory;
            }
        }

        throw new InvalidOperationException($"Maximum number of {numberOfPhotos} photos already reached for this {entityName}.");
    }

    public bool UploadImages(string fullFilePath, string fileNameOnDisk, IFormFile file)
    {
        try
        {
            using var stream = new FileStream(fullFilePath, FileMode.Create);
            file.CopyTo(stream);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException
            || ex is IOException)
        {
            {
                return false;
            }
        }
        return true;
    }

    public async Task<List<PhotoDTO?>>? GetAllPhotosByVehicleIdWithIncludesAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return ((await Repository.GetAllPhotosByVehicleIdWithIncludesAsync(vehicleId: vehicleId, roleName: roleName,
            userId: userId, noTracking: noTracking, noIncludes: noIncludes)).Select(p => Mapper.Map(p))).ToList();
    }

    public List<PhotoDTO?>? GetAllPhotosByVehicleIdWithIncludes(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllPhotosByVehicleIdWithIncludes(vehicleId, userId, roleName, noTracking, noIncludes).Select(p => Mapper.Map(p)).ToList();
    }

    public string[] GetFilesRelativePaths(IEnumerable<PhotoDTO?> photos) =>
        photos.Select(p => p!.PhotoURL ?? "").ToArray();

    public string[] GetFileNames(IEnumerable<PhotoDTO?> photos) =>
                photos.Select(p => p!.Title.Replace("_", " ") ?? "Unknown").ToArray();

    public async Task<string> CreateThumbnailAsync(string fullFilePath, string fileName, string fileExtension,
        string thumbFullFilePath, int? width = 300, int? height = 300)
    {
        using var image = await Image.LoadAsync(fullFilePath);
        var imagePath = Path.Combine(thumbFullFilePath, fileName);
        image.Mutate(async i =>
        {
            if (width.HasValue && height.HasValue)
            {
                i.Resize(new ResizeOptions()
                {
                    Size = new Size(height: height.Value, width: width.Value),
                    Mode = ResizeMode.Crop
                });

                IImageEncoder encoder = fileExtension switch
                {
                    ".jpg" => new JpegEncoder { Quality = 80 },
                    ".png" => new PngEncoder(),
                    _ => throw new NotSupportedException()
                };

                await image.SaveAsync(imagePath, encoder);
            }
        });
        return imagePath;
    }

    public bool HasAnyImages(IEnumerable<PhotoDTO?> photos)
    {
        return photos.Any() ? true : false;
    }

    public async Task<string?> GetDirectoryIdByAppUserIdAsStringAsync(Guid userId, string? roleName, bool noTracking = true, bool noIncludes = false)
    {
        return await Repository.GetDirectoryIdByAppUserIdAsStringAsync(userId: userId, roleName: roleName, noTracking, noIncludes);
    }

    public string? GetDirectoryIdByAppUserIdAsString(Guid userId, string? roleName, bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetDirectoryIdByAppUserIdAsString(userId: userId, roleName: roleName, noTracking, noIncludes);
    }

    public async Task<bool> IsPhotoOfVehicleAsync(Guid photoId, Guid vehicleId, Guid? userId = null,
        string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.IsPhotoOfVehicleAsync(photoId: photoId, vehicleId: vehicleId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes,
            showDeleted: showDeleted);
    }

    public bool IsPhotoOfVehicle(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.IsPhotoOfVehicle(photoId: photoId, vehicleId: vehicleId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }



    public async Task<bool> IsPhotoOfAdminAsync(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.IsPhotoOfAdminAsync(photoId: photoId, adminId: adminId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public bool IsPhotoOfAdmin(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.IsPhotoOfAdmin(photoId: photoId, adminId: adminId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public async Task<string?> GetAdminFirstAndLastNameAsync(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.GetAdminFirstAndLastNameAsync(photoId: photoId, adminId: adminId, userId: userId, roleName: roleName, noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public string? GetAdminFirstAndLastName(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.GetAdminFirstAndLastName(photoId: photoId, adminId: adminId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public async Task<string?> GetVehicleIdentifierAsync(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.GetVehicleIdentifierAsync(photoId: photoId, vehicleId: vehicleId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public string? GetVehicleIdentifier(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.GetVehicleIdentifier(photoId: photoId, vehicleId: vehicleId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }



    public IEnumerable<PhotoDTOGallery> GettingPhotosForGallery(IEnumerable<PhotoDTO?>? photos, Guid vehicleId)
    {

        var result = photos!.Select(p => new PhotoDTOGallery() { Id = p!.Id, Title = p.Title.Replace('_', ' ').Trim(), PhotoURL = p.PhotoURL, VehicleId = vehicleId, ThumbnailRelativePath = p.ThumbnailRelativePath!, });
        return result;
    }

    public async Task<bool> IsPhotoOfDriverAsync(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.IsPhotoOfDriverAsync(photoId: photoId, driverId: driverId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public bool IsPhotoOfDriver(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.IsPhotoOfDriver(photoId: photoId, driverId: driverId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public async Task<string?> GetDriverFirstAndLastNameAsync(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.GetDriverFirstAndLastNameAsync(photoId: photoId, driverId: driverId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public string? GetDriverFirstAndLastName(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.GetDriverFirstAndLastName(photoId: photoId, driverId: driverId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public async Task<bool> IsPhotoOfCustomerAsync(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.IsPhotoOfCustomerAsync(photoId: photoId, customerId: customerId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public bool IsPhotoOfCustomer(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.IsPhotoOfCustomer(photoId: photoId, customerId: customerId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public async Task<string?> GetCustomerFirstAndLastNameAsync(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return await Repository.GetCustomerFirstAndLastNameAsync(photoId: photoId, customerId: customerId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public string? GetCustomerFirstAndLastName(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        return Repository.GetCustomerFirstAndLastName(photoId: photoId, customerId: customerId, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted);
    }

    public bool DoAllPhotosBelongToDirectory(List<PhotoDTO?> photos, string directoryId)
    {
        return photos.TrueForAll(p => p!.DirectoryTitleId.Equals(directoryId));
    }

    public List<string?> GetThumbnailsFullPaths(List<PhotoDTO?> photos)
    {
        return photos.Select(p => p!.ThumbnailFullPath).ToList();
    }

    public List<string?> GetPhotosFullPaths(List<PhotoDTO?> photos)
    {
        return photos.Select(p => p!.PhotoFullPath).ToList();
    }
}

   