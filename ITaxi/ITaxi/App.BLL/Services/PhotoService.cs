using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;
using Microsoft.AspNetCore.Http;

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
        for (int i = 0; i < files.Count; i++)
        {
            if (files[i].Length <= 0
            || files[i].Length > 5000000
            || (!files[i].FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                && !files[i].FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

        }
        return true;
    }

    public bool IsDirectoryNameCorrect(string fileName, int? numberOfDirectoryNamePartsNeeded = 4)
    {
        int minimumLengthOfDirectoryNamePartsNeeded = 1;
        if (numberOfDirectoryNamePartsNeeded < minimumLengthOfDirectoryNamePartsNeeded)
            throw new ArgumentOutOfRangeException();
        var directoryNameParts = fileName.Split(" ");
        if (directoryNameParts.Length < numberOfDirectoryNamePartsNeeded)
            return false;
        else
        {
            GetDirectoryName(directoryNameParts);
            return true;
        }
    }

    public void GetDirectoryName(string[] directoryNameParts, int? numberOfDirectoryNameParts = 4)
    {
        int minimumLengthOfDirectoryNamePartsNeeded = 1;
        if (directoryNameParts.Length < minimumLengthOfDirectoryNamePartsNeeded)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (!numberOfDirectoryNameParts.HasValue)
        {
            throw new ArgumentOutOfRangeException();
        }
        string directoryName = string.Join("_", directoryNameParts.Take(numberOfDirectoryNameParts.Value));
        directoryName = string.Concat(directoryName.Split(Path.GetInvalidFileNameChars())).Trim();

    }

    public string GetUploadFolderPath(string wwwRootPath, string[] directoryNames)
    {
        throw new NotImplementedException();
    }
}
