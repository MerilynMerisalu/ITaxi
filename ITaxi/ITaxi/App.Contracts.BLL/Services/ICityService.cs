using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICityService: IEntityService<App.BLL.DTO.AdminArea.CityDTO>,
    ICityRepositoryCustom<App.BLL.DTO.AdminArea.CityDTO>
{
    
}