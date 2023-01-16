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
        throw new NotImplementedException();
    }

    public IEnumerable<RideTimeDTO?> GettingAllRideTimesWithoutIncludes(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<RideTimeDTO?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public RideTimeDTO? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTimeDTO rideTime, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string?> GettingAllSelectedRideTimes(RideTimeDTO rideTime, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public RideTimeDTO? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByBookingIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public RideTimeDTO? GettingFirstRideTimeByBookingId(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<RideTimeDTO>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime, Guid cityId, int numberOfPassengers,
        Guid vehicleType, bool defaultToNextAvailable, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public RideTimeDTO? GettingBestAvailableRideTime(DateTime pickUpDateAndTime, Guid cityId, int numberOfPassengers,
        Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        throw new NotImplementedException();
    }
}