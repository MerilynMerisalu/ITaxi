using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class RideTimeService: BaseEntityService<App.BLL.DTO.AdminArea.RideTimeDTO, App.DAL.DTO.AdminArea.RideTimeDTO, IRideTimeRepository>, 
    IRideTimeService
{
    
    public RideTimeService(IRideTimeRepository repository, IMapper<RideTimeDTO, DAL.DTO.AdminArea.RideTimeDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<RideTimeDTO>?> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllRideTimesWithoutIncludesAsync(noTracking))!.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<RideTimeDTO?> GettingAllRideTimesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllRideTimesWithoutIncludes(noTracking)!.Select(e => Mapper.Map(e))!;
    }

    public async Task<RideTimeDTO?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingRideTimeWithoutIncludesByIdAsync(id, noTracking));
    }

    public RideTimeDTO? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingRideTimeWithoutIncludesById(id, noTracking));
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedRideTimesAsync(userId, roleName, noTracking))!.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GettingAllOrderedRideTimes(userId, roleName, noTracking)!.Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedRideTimesWithoutIncludesAsync(noTracking))!.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllOrderedRideTimesWithoutIncludes(noTracking)!.Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTimeDTO rideTime, bool noTracking = true)
    {
        return await Repository.GettingAllSelectedRideTimesAsync(Mapper.Map(rideTime)!, noTracking);
    }

    public IEnumerable<string?> GettingAllSelectedRideTimes(RideTimeDTO rideTime, bool noTracking = true)
    {
        return Repository.GettingAllSelectedRideTimes(Mapper.Map(rideTime)!, noTracking);
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingFirstRideTimeByIdAsync(id, userId, roleName, noTracking));
    }

    public RideTimeDTO? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingFirstRideTimeById(id, userId, roleName, noTracking));
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByBookingIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingFirstRideTimeByBookingIdAsync(id, userId, roleName, noTracking));
    }

    public RideTimeDTO? GettingFirstRideTimeByBookingId(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingFirstRideTimeByBookingId(id, userId, roleName, noTracking));
    }

    public List<string> CalculatingRideTimes(Guid scheduleId)
    {
        return Repository.CalculatingRideTimes(scheduleId);
    }

    public async Task<IList<RideTimeDTO>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime, Guid cityId, int numberOfPassengers,
        Guid vehicleType, bool defaultToNextAvailable, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return (await Repository.GettingBestAvailableRideTimeAsync(pickUpDateAndTime, cityId, numberOfPassengers, vehicleType, defaultToNextAvailable, userId, roleName, noTracking))!.Select(e => Mapper.Map(e)).ToList()!;
    }

    public RideTimeDTO? GettingBestAvailableRideTime(DateTime pickUpDateAndTime, Guid cityId, int numberOfPassengers,
        Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingBestAvailableRideTime(pickUpDateAndTime, cityId, numberOfPassengers, userId, roleName, noTracking));
    }

    public List<string> GettingRemainingRideTimesByScheduleId(Guid scheduleId)
    {
        return Repository.GettingRemainingRideTimesByScheduleId(scheduleId);
    }

    public async Task<bool> HasScheduleAnyAsync(Guid id, bool noTracking = true)
    {
        return await Repository.HasScheduleAnyAsync(id, noTracking);
    }

    public bool HasScheduleAny(Guid id, bool noTracking = true)
    {
        return Repository.HasScheduleAny(id, noTracking);
    }
}