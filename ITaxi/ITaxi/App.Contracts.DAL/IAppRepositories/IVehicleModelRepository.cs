using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleModelRepository : IEntityRepository<VehicleModelDTO>, IVehicleModelRepositoryCustom<VehicleModelDTO>
{
    
}

public interface IVehicleModelRepositoryCustom<TEntity> 
{
    Task<IEnumerable<TEntity>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true);
    Task<TEntity?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true);
    TEntity? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true);
    Task<List<TEntity>> GettingVehicleModelsByMarkIdAsync(Guid markId, bool noTracking = true);
    List<TEntity> GettingVehicleModels(Guid markId, bool noTracking = true);
}