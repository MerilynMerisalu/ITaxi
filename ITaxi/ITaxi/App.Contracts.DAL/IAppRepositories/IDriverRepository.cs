using App.DAL.DTO.AdminArea;

using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverRepository : IEntityRepository<DriverDTO>, IDriverRepositoryCustom<DriverDTO>
{
    
}

public interface IDriverRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllDriversOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllDriversOrderedByLastName(bool noTracking = true);
    Task<TEntity> GettingDriverByVehicleAsync(Guid driverAppUserId, bool noTracking = true);
}