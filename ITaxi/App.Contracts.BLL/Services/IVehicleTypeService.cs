using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVehicleTypeService: IEntityService<App.BLL.DTO.AdminArea.VehicleTypeDTO>,
    IVehicleTypeRepositoryCustom<App.BLL.DTO.AdminArea.VehicleTypeDTO>
{
    
}