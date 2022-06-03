using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleModelRepository: IEntityRepository<VehicleModel>
{
    Task<IEnumerable<VehicleModel>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true);
    IEnumerable<VehicleModel> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true);
    Task<VehicleModel?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true);
    VehicleModel? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true);
    Task<IEnumerable<VehicleModel>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(bool noTracking = true);
    IEnumerable<VehicleModel> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true);
}