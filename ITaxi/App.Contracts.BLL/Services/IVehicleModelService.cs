using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVehicleModelService : IEntityService<App.BLL.DTO.AdminArea.VehicleModelDTO>, 
    IVehicleModelRepositoryCustom<App.BLL.DTO.AdminArea.VehicleModelDTO>
{
    
}