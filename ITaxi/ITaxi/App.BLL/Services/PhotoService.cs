using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;
using Microsoft.AspNetCore.Http;
using System.IO;

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
            .Select(e => Mapper.Map(e));
    }

    public IEnumerable<PhotoDTO?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GetAllPhotosWithIncludes(userId, roleName, noTracking).Select(e => Mapper.Map(e));
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
        bool noTracking = true)
    {
        return await Repository.GetPhotoCountByVehicleIdAsync(vehicleId, userId, roleName, noTracking);
    }

    public int GetPhotoCountByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GetPhotoCountByVehicleId(vehicleId, userId, roleName, noTracking);
    }

    public bool AlreadyHasACertainNumberOfImages(int numberOfImages, int? numberOfImagesAllowed = 4, List<IFormFile>? files = null)
    {
        int minimumNumberOfImagesAllowed = 1;
        if (files == null) throw new ArgumentNullException(nameof(files));

        else if (numberOfImagesAllowed < minimumNumberOfImagesAllowed)
            throw new ArgumentOutOfRangeException(nameof(numberOfImagesAllowed));

        else if ((files.Count + numberOfImages) > numberOfImagesAllowed)
            return true;
        return false;
    }

    public bool AreAllFilesCorrect(List<IFormFile> files)
    {
        foreach (IFormFile file in files)
        {
            if (file == null) continue;
            if (file.Length <= 0 || file.Length > 5_000_000)
                return false;
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg")
                return false;
        }
        return true;
    }


   
    public bool DoesFileExist(string fullFilePath)
    {
        if (!File.Exists(fullFilePath)) 
            return false;
        return true;
    }

    public string? GetDirectoryPath(string startOfDirectoryPath, string[]? middleParts, string? directoryName = null)
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
      return Repository.GetDirectoryIdByVehicleIdAsString(vehicleId, userId,roleName, noTracking, noIncludes);
    }

    public async Task<bool> UploadImagesAsync(string fullFilePath, string fileNameOnDisk, IFormFile file)
    {
        fullFilePath = Path.Combine(fullFilePath, fileNameOnDisk);
        
        try
        {
         await using var stream = new FileStream(fullFilePath, FileMode.Create);
         await file.CopyToAsync(stream);
     
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException
            || ex is IOException )
        {
            {
                return false;
            }
        }
        return true;
    }

    public string FileNameFormat(string fileName, int maxLength)
    {
        
        if(fileName.Length > maxLength )
            fileName = fileName.Substring(0, maxLength) + "...";
        return fileName.Replace(" ", "_").Trim();
    }

    public string GetFileNameForDirectory(string fullUploadDirectoryPath, string fileExtension)
    {
        const string filenameBeginning = "Foto_";
        fullUploadDirectoryPath = fullUploadDirectoryPath.TrimEnd(Path.DirectorySeparatorChar);

        for (int i = 1; i < 5; i++)
        {
            string fileNameInDirectory = $"{filenameBeginning}{i}{fileExtension}";
            string fullPath = Path.Combine(fullUploadDirectoryPath, fileNameInDirectory);

            if (!DoesFileExist(fullPath))
            {
                return fileNameInDirectory;
            }
        }

        throw new InvalidOperationException("Maximum number of 4 photos already reached for this vehicle.");
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
}
