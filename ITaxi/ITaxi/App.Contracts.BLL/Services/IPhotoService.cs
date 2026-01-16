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
    string? CreateDirectoryPath(string startOfDirectoryPath, string[]? middleParts, string? directoryName = null);
    bool DoesDirectoryExist(string directoryPath);
    void CreateDirectory(string directoryPath);
    bool DoesFileExist(string fullFilePath);
    Task<bool> UploadImagesAsync(string fullUploadDirectoryPath, string fileNameOnDisk, IFormFile file);
    bool UploadImages(string fullFilePath, string fileNameOnDisk, IFormFile file);
    string FileNameFormat(string fileName, int maxLength);
    string GetFileNameForDirectory(string fullUploadDirectoryPath, string fileExtension, int? numberOfPhotos = 4, string? entityName = "vehicle");
    string[]GetFilesRelativePaths(IEnumerable<PhotoDTO?> photos);
   
    string[] GetFileNames(IEnumerable<PhotoDTO?> photos);
    Task<string> CreateThumbnailAsync(string fullFilePath, string fileName, string fileExtension, string thumbFullFilePath, 
                            int? width = 300, int? height = 300);
   bool HasAnyImages(IEnumerable<PhotoDTO?> photos);
   //Task<IEnumerable<PhotoDTOGallery>> GettingPhotosForGalleryAsync(List<PhotoDTOGallery>? photos, Guid vehicleId);
   IEnumerable<PhotoDTOGallery> GettingPhotosForGallery(IEnumerable<PhotoDTO?>? photos, Guid vehicleId);
   bool DoAllPhotosBelongToDirectory(List<PhotoDTO?> photos, string directoryId);
  List<string?> GetThumbnailsFullPaths(List<PhotoDTO?> photos);
  List<string?> GetPhotosFullPaths(List<PhotoDTO?> photos);
 public readonly record struct ImageSize(int Width, int Height); 

}