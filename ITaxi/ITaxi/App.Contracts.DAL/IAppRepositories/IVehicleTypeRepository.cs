using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleTypeRepository : IEntityRepository<VehicleType>
{
    Task<IEnumerable<VehicleType>> GetAllVehicleTypesOrderedAsync(bool noTracking = true);
    IEnumerable<VehicleType> GetAllVehicleTypesOrdered(bool noTracking = true);
}