using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ScheduleService: BaseEntityService<ScheduleDTO, App.DAL.DTO.AdminArea.ScheduleDTO, 
    IScheduleRepository>, IScheduleService
{
    public ScheduleService(IScheduleRepository repository, 
        IMapper<ScheduleDTO, DAL.DTO.AdminArea.ScheduleDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<ScheduleDTO>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GetAllSchedulesWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GetAllSchedulesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GetAllSchedulesWithoutIncludes(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithIncludesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedSchedulesWithIncludesAsync(userId, roleName, noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true)
    {
        return Repository.GettingAllOrderedSchedulesWithIncludes(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedSchedulesWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllOrderedSchedulesWithIncludes(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<ScheduleDTO?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingScheduleWithoutIncludesAsync(id, noTracking));
    }

    public ScheduleDTO? GetScheduleWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GetScheduleWithoutIncludes(id, noTracking));
    }

    public async Task<ScheduleDTO?> GettingTheFirstScheduleByIdAsync(Guid id, Guid? userid = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingTheFirstScheduleByIdAsync(id, userid, roleName, noTracking));
    }

    public ScheduleDTO? GettingTheFirstScheduleById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingTheFirstScheduleById(id, userId, roleName, noTracking));
    }

    public async Task<ScheduleDTO?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingTheFirstScheduleAsync(userid, roleName, noTracking));
    }

    public ScheduleDTO? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingTheFirstSchedule(userId, roleName, noTracking));
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingTheScheduleByDriverIdAsync(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return (await Repository.GettingTheScheduleByDriverIdAsync(driverId, userId, roleName, noTracking))
            .Select(e=> Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingTheScheduleByDriverId(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Repository.GettingTheScheduleByDriverId(driverId, userId, roleName, noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    //public DateTime[] GettingStartAndEndTime(Schedule[] schedules, Guid? userId = null,
    //    string? roleName = null)
    //{
    //    return Repository.GettingStartAndEndTime(schedules.Select(s => Mapper.Map(s)), userId, roleName);
    //}

    public int NumberOfRideTimes(Guid? driverId = null, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.NumberOfRideTimes(driverId, userId, roleName, noTracking);
    }

    public int NumberOfTakenRideTimes(Guid? driverId = null, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.NumberOfTakenRideTimes(driverId, userId, roleName, noTracking);
    }
}