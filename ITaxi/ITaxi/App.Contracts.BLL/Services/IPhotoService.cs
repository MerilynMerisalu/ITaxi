using App.BLL.DTO.AdminArea;
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
    Task<bool> UploadImagesAsync(string fullUploadDirectoryPath, string fileNameOnDisk, IFormFile file);
    bool UploadImages(string fullFilePath, string fileNameOnDisk, IFormFile file);
    string FileNameFormat(string fileName, int maxLength);
    string GetFileNameForDirectory(string fullUploadDirectoryPath, string fileExtension);
    string[]GetFilesRelativePaths(IEnumerable<PhotoDTO?> photos);
    string[] GetFileNames(IEnumerable<PhotoDTO?> photos);


}