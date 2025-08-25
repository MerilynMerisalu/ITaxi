using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;
using Microsoft.AspNetCore.Http;

namespace App.Contracts.BLL.Services;

public interface IPhotoService: IEntityService<App.BLL.DTO.AdminArea.PhotoDTO>, 
    IPhotoRepositoryCustom<App.BLL.DTO.AdminArea.PhotoDTO>
{
    bool AlreadyHasACertainNumberOfImages
    (
        int numberOfImages,
        int? numberOfImagesAllowed = 4,
        List<IFormFile>? files = null);
    
    bool AreAllFilesCorrect(List<IFormFile> files);
    string? GetDirectoryPath(string startOfDirectoryPath, string[]? middleParts, string? directoryName = null);
    bool DoesDirectoryExist(string directoryPath);
    void CreateDirectory(string directoryPath);
    bool DoesFileExist(string fullFilePath);
    Task<bool> UploadImagesAsync(string fullUploadDirectoryPath,string fileName, IFormFile file);
    bool UploadImages(string fullUploadDirectoryPath, string fileName, IFormFile file);

    
}