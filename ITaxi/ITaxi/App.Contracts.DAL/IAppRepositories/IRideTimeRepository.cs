using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IRideTimeRepository: IEntityRepository<RideTime>
{
    Task<IEnumerable<RideTime>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<RideTime> GettingAllRideTimesWithoutIncludes(bool noTracking = true);
    Task<RideTime?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    RideTime? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true);
    
}