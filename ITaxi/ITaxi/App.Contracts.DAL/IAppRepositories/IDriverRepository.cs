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
    Task<TEntity> GettingDriverByVehicleAsync(Guid driverId, bool noTracking = true);
    Task<bool> HasAnySchedulesAsync(Guid driverId, bool noTracking = true);
    bool HasAnySchedules(Guid driverId, bool noTracking = true);
    Task<bool> HasAnyBookingsAsync(Guid driverId, bool noTracking = true);
    bool HasAnyBookings(Guid driverId, bool noTracking = true);

}