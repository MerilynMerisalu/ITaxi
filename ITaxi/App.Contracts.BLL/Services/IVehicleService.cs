using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVehicleService : IEntityService<App.BLL.DTO.AdminArea.VehicleDTO>, 
    IVehicleRepositoryCustom<App.BLL.DTO.AdminArea.VehicleDTO>
{
    
}