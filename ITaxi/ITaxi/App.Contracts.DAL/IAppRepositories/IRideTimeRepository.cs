using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IRideTimeRepository : IEntityRepository<RideTimeDTO>
{
    Task<IEnumerable<RideTimeDTO?>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<RideTimeDTO?> GettingAllRideTimesWithoutIncludes(bool noTracking = true);
    Task<RideTimeDTO?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    RideTimeDTO? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true);

    Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimes(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTimeDTO rideTime, bool noTracking = true);
    IEnumerable<string?> GettingAllSelectedRideTimes(RideTimeDTO rideTime, bool noTracking = true);

    Task<RideTimeDTO?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    RideTimeDTO? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<RideTimeDTO?> GettingFirstRideTimeByBookingIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    RideTimeDTO? GettingFirstRideTimeByBookingId(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime);

    //string DriveTimeFormatting(RideTime rideTime);
    Task<IList<RideTimeDTO>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime, Guid cityId,
        int numberOfPassengers,
        Guid vehicleType,
        bool defaultToNextAvailable,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true); 

    RideTimeDTO? GettingBestAvailableRideTime(DateTime pickUpDateAndTime, Guid cityId,
        int numberOfPassengers, Guid? userId = null, string? roleName = null,
        bool noTracking = true); 
}