using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleModelRepository : IEntityRepository<VehicleModelDTO>
{
    Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true);
    IEnumerable<VehicleModelDTO> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true);
    Task<VehicleModelDTO?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true);
    VehicleModelDTO? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true);
    Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(bool noTracking = true);
    IEnumerable<VehicleModelDTO> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true);

    Task<List<VehicleModelDTO>> GettingVehicleModelsByMarkIdAsync(Guid markId, bool noTracking = true);
    List<VehicleModelDTO> GettingVehicleModels(Guid markId, bool noTracking = true);
}