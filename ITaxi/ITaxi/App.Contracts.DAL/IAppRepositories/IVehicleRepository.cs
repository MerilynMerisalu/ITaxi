using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleRepository: IEntityRepository<Vehicle>
{
    Task<IEnumerable<Vehicle>> GettingVehicleOrderedAsync(bool noTracking = true);
    IEnumerable<Vehicle> GettingVehicleOrdered(bool noTracking = true);
    List<int> GettingManufactureYears();
    
}