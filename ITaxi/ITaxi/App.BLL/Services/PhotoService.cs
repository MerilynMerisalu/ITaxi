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
        return files.All(t => t.Length > 0 && t.Length <= 5000000 && (t.FileName.EndsWith(".jpg", 
            StringComparison.OrdinalIgnoreCase) || t.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)));
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

    public async Task<string?> GetDiretoryIdByVehicleIdAsStringAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
        return await Repository.GetDiretoryIdByVehicleIdAsStringAsync(vehicleId, userId, roleName, noTracking, noIncludes);
    }

    public string? GetDiretoryIdByVehicleIdAsString(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
      return Repository.GetDiretoryIdByVehicleIdAsString(vehicleId, userId,roleName, noTracking, noIncludes);
    }

    public async Task<bool> UploadImagesAsync(string fullUploadDirectoryPath, IFormFile file)
    {
        try
        {
            
         await using var stream = new FileStream(fullUploadDirectoryPath, FileMode.Create);
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

    public bool UploadImages(string fullUploadDirectoryPath, IFormFile file)
    {
        try
        {
             using var stream = new FileStream(fullUploadDirectoryPath, FileMode.Create);
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
