using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleRepository : IEntityRepository<Vehicle>
{
    Task<IEnumerable<Vehicle>> GettingOrderedVehiclesAsync(Guid? userId = null,
        string? roleNames = null, bool noTracking = true);

    IEnumerable<Vehicle> GettingOrderedVehicles(Guid? userId = null, string? roleName = null, bool noTracking = true);
    Task<IEnumerable<Vehicle>> GettingVehiclesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Vehicle> GettingVehiclesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Vehicle>> GettingOrderedVehiclesWithoutIncludesAsync(Guid? userId = null, string? roleName = null,  bool noTracking = true);
    IEnumerable<Vehicle> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true);

    Task<Vehicle?> GettingVehicleWithIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Vehicle? GettingVehicleById(Guid id, bool noTracking = true);
    Task<Vehicle?> GettingVehicleWithoutIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    Vehicle? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true);
    List<int> GettingManufactureYears();
    Task<Vehicle?> GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(Booking booking);
    Vehicle? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(Booking booking);
}