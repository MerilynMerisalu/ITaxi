using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVehicleMarkService: IEntityService<App.BLL.DTO.AdminArea.VehicleMarkDTO>,
    IVehicleMarkRepositoryCustom<App.BLL.DTO.AdminArea.VehicleMarkDTO>
{
    
}