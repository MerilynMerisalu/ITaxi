using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleMarkRepository : IEntityRepository<VehicleMark>
{
    Task<IEnumerable<VehicleMark>> GetAllVehicleMarkOrderedAsync(bool noTracking = true);
    IEnumerable<VehicleMark> GetAllVehicleMarkOrdered(bool noTracking = true);
}