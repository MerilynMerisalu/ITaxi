using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleRepository : IEntityRepository<VehicleDTO>, IVehicleRepositoryCustom<VehicleDTO>
{
    
}

public interface IVehicleRepositoryCustom<TEntity> 
{
    Task<IEnumerable<TEntity>> GettingOrderedVehiclesAsync(Guid? userId = null,
        string? roleNames = null, bool noTracking = true);
    IEnumerable<TEntity> GettingOrderedVehicles(Guid? userId = null, string? roleName = null, bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingVehiclesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GettingVehiclesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingOrderedVehiclesWithoutIncludesAsync(Guid? userId = null, string? roleName = null,  bool noTracking = true);
    IEnumerable<TEntity> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true);
    Task<TEntity?> GettingVehicleWithIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);
    Task<TEntity?> GettingVehicleWithoutIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    TEntity? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true);
    List<int> GettingManufactureYears();
    Task<TEntity?> GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(Booking booking);
    TEntity? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(Booking booking);
    Task<List<TEntity>> GettingVehiclesByDriverIdAsync(Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true);
    List<TEntity> GettingVehiclesByDriverId(Guid driverId, bool noTracking = true);
    Task<bool> HasAnyVehicleModelsAnyAsync(Guid vehicleModelId, bool noTracking = true);
}