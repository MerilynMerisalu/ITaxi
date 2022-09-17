﻿using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IScheduleRepository : IEntityRepository<Schedule>
{
    Task<IEnumerable<Schedule>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Schedule> GetAllSchedulesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithIncludesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true);

    IEnumerable<Schedule> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true);
    Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Schedule> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true);
    Task<Schedule?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true);

    Schedule? GetScheduleWithoutIncludes(Guid id, bool noTracking = true);

    Task<Schedule?> GettingTheFirstScheduleByIdAsync(Guid id,Guid? userid = null, string? roleName = null, bool noTracking = true);
    Schedule? GettingTheFirstScheduleById(Guid id ,Guid? userId = null, string? roleName = null, bool noTracking = true);
    
    Task<Schedule?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true);
    Schedule? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true);


    DateTime[] GettingStartAndEndTime(IEnumerable<Schedule> schedules, Guid? userId = null, string? roleName = null);
    //Task<int> NumberOfRideTimesPerScheduleAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
   // int NumberOfRideTimesPerSchedule(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
}