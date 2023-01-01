using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAdminService: IEntityService<App.BLL.DTO.AdminArea.AdminDTO>, 
    IAdminRepositoryCustom<App.BLL.DTO.AdminArea.AdminDTO>
{
    
}