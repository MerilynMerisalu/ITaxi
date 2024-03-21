using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IRideTimeRepository : IEntityRepository<RideTimeDTO>, IRideTimeRepositoryCustom<RideTimeDTO>
{
   
}

public interface IRideTimeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity?> GettingAllRideTimesWithoutIncludes(bool noTracking = true);
    Task<TEntity?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    TEntity? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true);

    Task<IEnumerable<TEntity?>> GettingAllOrderedRideTimesAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    IEnumerable<TEntity?> GettingAllOrderedRideTimes(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<IEnumerable<TEntity?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(TEntity rideTime, bool noTracking = true);
    IEnumerable<string?> GettingAllSelectedRideTimes(TEntity rideTime, bool noTracking = true);

    Task<TEntity?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false);

    TEntity? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false);

    Task<TEntity?> GettingFirstRideTimeByBookingIdAsync(Guid id, Guid? userId = null, 
        string? roleName = null,
        bool noTracking = true, bool noIncludes = false);

    TEntity? GettingFirstRideTimeByBookingId(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false);

    List<string> CalculatingRideTimes(Guid scheduleId);
    //List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime);

    //string DriveTimeFormatting(RideTime rideTime);
    Task<IList<TEntity>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime, Guid cityId,
        int numberOfPassengers,
        Guid vehicleType,
        bool defaultToNextAvailable,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true); 

    TEntity? GettingBestAvailableRideTime(DateTime pickUpDateAndTime, Guid cityId,
        int numberOfPassengers, Guid? userId = null, string? roleName = null,
        bool noTracking = true); 
    
    List<string> GettingRemainingRideTimesByScheduleId(Guid scheduleId);
    Task<bool> HasScheduleAnyAsync(Guid id, bool noTracking = true);
    bool HasScheduleAny(Guid id, bool noTracking = true);
    Task<IEnumerable<string>>? GettingAllRideTimesAvailableAsync(
        Guid scheduleId, Guid userId, string? roleName = null,
        bool noTracking = true);
}