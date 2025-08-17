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
}