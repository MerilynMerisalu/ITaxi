using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleRepository: IEntityRepository<Vehicle>
{
    Task<IEnumerable<Vehicle>> GettingOrderedVehiclesAsync(bool noTracking = true);
    IEnumerable<Vehicle> GettingOrderedVehicles(bool noTracking = true);
    Task<IEnumerable<Vehicle>> GettingVehiclesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Vehicle> GettingVehiclesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Vehicle>> GettingOrderedVehiclesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Vehicle> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true);
    Task<Vehicle?> GettingVehicleByIdAsync(Guid id, bool noTracking = true);
    Vehicle? GettingVehicleById(Guid id, bool noTracking = true);
    Task<Vehicle?> GettingVehicleWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    Vehicle? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true);
    List<int> GettingManufactureYears();
    Task<Vehicle?> GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(Booking booking);
    Vehicle? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(Booking booking);
}