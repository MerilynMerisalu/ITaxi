using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleMarkRepository : IEntityRepository<VehicleMarkDTO>, IVehicleMarkRepositoryCustom<VehicleMarkDTO>
{
    
}

public interface IVehicleMarkRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllVehicleMarkOrderedAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllVehicleMarkOrdered(bool noTracking = true);
}