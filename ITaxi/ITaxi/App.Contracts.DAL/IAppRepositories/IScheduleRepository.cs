using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IScheduleRepository : IEntityRepository<ScheduleDTO>, IScheduleRepositoryCustom<ScheduleDTO>
{
    
    
    
}

public interface IScheduleRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllSchedulesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<TEntity>> GettingAllOrderedSchedulesWithIncludesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    IEnumerable<TEntity> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true);
    Task<TEntity?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true);

    TEntity? GetScheduleWithoutIncludes(Guid id, bool noTracking = true);

    Task<TEntity?> GettingTheFirstScheduleByIdAsync(Guid id, Guid? userid = null, string? roleName = null,
        bool noTracking = true);

    TEntity? GettingTheFirstScheduleById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<TEntity?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true);
    TEntity? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true);

    Task<IEnumerable<TEntity>> GettingTheScheduleByDriverIdAsync(Guid driverId, Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    IEnumerable<TEntity> GettingTheScheduleByDriverId(Guid driverId, Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    DateTime[] GettingStartAndEndTime(IEnumerable<TEntity> schedules, Guid? userId = null, string? roleName = null);


    /*int NumberOfRideTimes(Guid? driverId = null, Guid? userId = null,
        string? roleName = null, bool noTracking = true);
    int NumberOfTakenRideTimes(Guid? driverId = null, Guid? userId = null,
        string? roleName = null, bool noTracking = true);
        */
}