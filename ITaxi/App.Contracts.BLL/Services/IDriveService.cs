using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IDriveService: IEntityService<App.BLL.DTO.AdminArea.DriveDTO>, 
    IDriveRepositoryCustom<App.BLL.DTO.AdminArea.DriveDTO>
{
    
}