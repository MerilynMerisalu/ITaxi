using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPhotoService: IEntityService<App.BLL.DTO.AdminArea.PhotoDTO>, 
    IPhotoRepositoryCustom<App.BLL.DTO.AdminArea.PhotoDTO>
{
    
}