using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IScheduleRepository: IEntityRepository<Schedule>
{
   Task<IEnumerable<Schedule>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true);
   IEnumerable<Schedule> GetAllSchedulesWithoutIncludes(bool noTracking = true);
   Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithIncludesAsync(bool noTracking = true);
   IEnumerable<Schedule> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true);
   Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true);
   IEnumerable<Schedule> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true);
   Task<Schedule?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true);
   Schedule? GetScheduleWithoutIncludes(Guid id, bool noTracking = true);
   DateTime SettingScheduleStartDateAndTime();
   DateTime SettingScheduleEndDateAndTime();
   Task<Schedule?> GettingTheFirstScheduleAsync(bool noTracking = true);
   Schedule? GettingTheFirstSchedule(bool noTracking = true);
   DateTime[] GettingStartAndEndTime();
   Task<Guid> GettingScheduleByDriverIdAsync(Guid driverId);
   Guid GettingScheduleByDriverId(Guid driverId);



}