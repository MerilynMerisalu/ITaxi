using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ScheduleService: BaseEntityService<App.BLL.DTO.AdminArea.ScheduleDTO, App.DAL.DTO.AdminArea.ScheduleDTO, 
    IScheduleRepository>, IScheduleService
{
    public ScheduleService(IScheduleRepository repository, IMapper<ScheduleDTO, DAL.DTO.AdminArea.ScheduleDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<ScheduleDTO>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GetAllSchedulesWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e))!;
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
        return Repository.GettingAllOrderedSchedulesWithIncludes(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedSchedulesWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllOrderedSchedulesWithIncludes(noTracking).Select(e => Mapper.Map(e))!
    }

    public async Task<ScheduleDTO?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingScheduleWithoutIncludesAsync(id, noTracking));
    }

    public ScheduleDTO? GetScheduleWithoutIncludes(Guid id, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<ScheduleDTO?> GettingTheFirstScheduleByIdAsync(Guid id, Guid? userid = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public ScheduleDTO? GettingTheFirstScheduleById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<ScheduleDTO?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public ScheduleDTO? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingTheScheduleByDriverIdAsync(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ScheduleDTO> GettingTheScheduleByDriverId(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public DateTime[] GettingStartAndEndTime(IEnumerable<ScheduleDTO> schedules, Guid? userId = null, string? roleName = null)
    {
        throw new NotImplementedException();
    }
}