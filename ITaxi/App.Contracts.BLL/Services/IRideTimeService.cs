using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRideTimeService: IEntityService<App.BLL.DTO.AdminArea.RideTimeDTO>, IRideTimeRepositoryCustom<App.BLL.DTO.AdminArea.RideTimeDTO>
{
    
}