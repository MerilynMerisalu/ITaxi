using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IRideTimeRepository : IEntityRepository<RideTime>
{
    Task<IEnumerable<RideTime?>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<RideTime?> GettingAllRideTimesWithoutIncludes(bool noTracking = true);
    Task<RideTime?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    RideTime? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true);

    Task<IEnumerable<RideTime?>> GettingAllOrderedRideTimesAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    IEnumerable<RideTime?> GettingAllOrderedRideTimes(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<IEnumerable<RideTime?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<RideTime?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTime rideTime, bool noTracking = true);
    IEnumerable<string?> GettingAllSelectedRideTimes(RideTime rideTime, bool noTracking = true);

    Task<RideTime?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    RideTime? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime);
    //string DriveTimeFormatting(RideTime rideTime);
}