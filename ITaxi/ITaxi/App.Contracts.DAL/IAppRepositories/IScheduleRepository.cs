using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IScheduleRepository : IEntityRepository<ScheduleDTO>
{
    Task<IEnumerable<ScheduleDTO>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<ScheduleDTO> GetAllSchedulesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithIncludesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true);
    Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true);
    Task<ScheduleDTO?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true);

    ScheduleDTO? GetScheduleWithoutIncludes(Guid id, bool noTracking = true);

    Task<ScheduleDTO?> GettingTheFirstScheduleByIdAsync(Guid id, Guid? userid = null, string? roleName = null,
        bool noTracking = true);

    ScheduleDTO? GettingTheFirstScheduleById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<ScheduleDTO?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true);
    ScheduleDTO? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true);

    Task<IEnumerable<ScheduleDTO>> GettingTheScheduleByDriverIdAsync(Guid driverId, Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    IEnumerable<ScheduleDTO> GettingTheScheduleByDriverId(Guid driverId, Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    DateTime[] GettingStartAndEndTime(IEnumerable<Schedule> schedules, Guid? userId = null, string? roleName = null);

    
    /*int NumberOfRideTimes(Guid? driverId = null, Guid? userId = null,
        string? roleName = null, bool noTracking = true);
    int NumberOfTakenRideTimes(Guid? driverId = null, Guid? userId = null,
        string? roleName = null, bool noTracking = true);
        */
    
    
}