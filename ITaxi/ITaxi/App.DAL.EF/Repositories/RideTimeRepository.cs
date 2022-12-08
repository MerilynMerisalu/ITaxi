using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RideTimeRepository : BaseEntityRepository<RideTime, AppDbContext>, IRideTimeRepository
{
    public RideTimeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<RideTime?>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<RideTime?> GettingAllRideTimesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<RideTime?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(r => r.ScheduleId.Equals(id));
    }

    public RideTime? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(r => r.ScheduleId.Equals(id));
    }

    public async Task<IEnumerable<RideTime?>> GettingAllOrderedRideTimesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking)
            .OrderBy(r => r.Schedule!.StartDateAndTime.Date)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Day)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Month)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Year)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Hour)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Minute)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Date)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Month)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Year)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Hour)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Minute)
            .ThenBy(r => r.RideDateTime.Date)
            .ThenBy(r => r.RideDateTime.Day)
            .ThenBy(r => r.RideDateTime.Month)
            .ThenBy(r => r.RideDateTime.Year)
            .ThenBy(r => r.RideDateTime.Hour)
            .ThenBy(r => r.RideDateTime.Minute)
            .ToListAsync();
    }

    public IEnumerable<RideTime?> GettingAllOrderedRideTimes(Guid? userId, string? roleName = null,
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking)
            .OrderBy(r => r.Schedule!.StartDateAndTime.Date)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Day)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Month)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Year)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Hour)
            .ThenBy(r => r.Schedule!.StartDateAndTime.Minute)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Date)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Month)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Year)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Hour)
            .ThenBy(r => r.Schedule!.EndDateAndTime.Minute)
            .ThenBy(r => r.RideDateTime.Date)
            .ThenBy(r => r.RideDateTime.Day)
            .ThenBy(r => r.RideDateTime.Month)
            .ThenBy(r => r.RideDateTime.Year)
            .ThenBy(r => r.RideDateTime.Hour)
            .ThenBy(r => r.RideDateTime.Minute)
            .ToList();
    }

    public async Task<IEnumerable<RideTime?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .OrderBy(r => r.RideDateTime.Date)
            .ThenBy(r => r.RideDateTime.Day)
            .ThenBy(r => r.RideDateTime.Month)
            .ThenBy(r => r.RideDateTime.Year)
            .ThenBy(r => r.RideDateTime.Hour)
            .ThenBy(r => r.RideDateTime.Minute)
            .ToListAsync();
    }

    public IEnumerable<RideTime?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking)
            .OrderBy(r => r.RideDateTime.Date)
            .ThenBy(r => r.RideDateTime.Day)
            .ThenBy(r => r.RideDateTime.Month)
            .ThenBy(r => r.RideDateTime.Year)
            .ThenBy(r => r.RideDateTime.Hour)
            .ThenBy(r => r.RideDateTime.Minute)
            .ToList();
    }

    public async Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTime rideTime, bool noTracking = true)
    {
        return await RepoDbSet
            .Where(r => r.ScheduleId.Equals(rideTime.ScheduleId))
            .Select(r => r.RideDateTime.ToString("t"))
            .ToListAsync();
    }

    public IEnumerable<string?> GettingAllSelectedRideTimes(RideTime rideTime, bool noTracking = true)
    {
        return RepoDbSet
            .Where(r => r.ScheduleId.Equals(rideTime.ScheduleId))
            .Select(r => r.RideDateTime.ToString("t"))
            .ToList();
    }

    public async Task<RideTime?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking)
            .FirstOrDefaultAsync(r => r.Id.Equals(id));
    }

    public RideTime? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking).FirstOrDefault(r => r.Id.Equals(id));
    }

    public List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime)
    {
        var times = new List<string>();
        var start = scheduleStartAndEndTime[0];
        var time = start;
        var end = scheduleStartAndEndTime[1];

        while (time < end)
        {
            times.Add(time.ToString("t"));
            time = time.AddMinutes(45);
        }


        return times;
    }

    /// <summary>
    /// Return the nearest available ride time that matches the <paramref name="cityId"/> and <paramref name="vehicleType"/>
    /// and has the required number of seats available, from all ride times
    /// if there is no match within 1 hour (either side) of the requested ride time, then return the previous and the next ride time in the future. 
    /// </summary>
    /// <param name="pickUpDateAndTime">Expected Pickup Time</param>
    /// <param name="cityId">City to filter the ride times by</param>
    /// <param name="numberOfPassengers">number of passengers that are travelling, vehicle needs this many available seats (+ the dirver)</param>
    /// <param name="vehicleType">Type of vehicle to filter the results by</param>
    /// <param name="defaultToNextAvailable">Flag to indicate if the "next" available ride time should be returned if there is no match within 1 hour of the request.</param>
    /// <param name="userId">Id of the caller, or NULL if this is an Admin request</param>
    /// <param name="roleName">Security Role of the caller, or NULL if this is an Admin request</param>
    /// <param name="noTracking">Flag to indicate if the DbContext should track this data for changes</param>
    /// <returns>Nearest time if one is available, or if <paramref name="defaultToNextAvailable"/> is true, then return 2, one before and one after</returns>
    public async Task<IList<RideTime>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime,
        Guid cityId,
        int numberOfPassengers,
        Guid vehicleType,
        bool defaultToNextAvailable,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true  )
    {
        var minTime = pickUpDateAndTime.AddMinutes(-5);
        var maxTime = pickUpDateAndTime.AddMinutes(5);
        var timeMinusOne = pickUpDateAndTime.AddMinutes(-1);
        var timePlusOne = pickUpDateAndTime.AddMinutes(1);
        var rideTimesQuery = CreateQuery(userId, roleName)
            .Where(rt => rt.IsTaken == false)
            .Where(rt => rt.Driver!.CityId.Equals(cityId))
            .Where(rt => rt.Schedule!.Vehicle!.VehicleTypeId.Equals(vehicleType))
            .Where(rt => rt.Schedule!.Vehicle!.NumberOfSeats > numberOfPassengers);
        
        var closestRideTimes = await rideTimesQuery
            .Where(rt => rt.Schedule!.StartDateAndTime <= timePlusOne 
                             && rt.Schedule.EndDateAndTime >= timeMinusOne
                             && rt.RideDateTime >= minTime 
                             && rt.RideDateTime <= maxTime)
            .ToListAsync();
        closestRideTimes = closestRideTimes   
            .OrderBy(x => Math.Abs(pickUpDateAndTime.Subtract(x.RideDateTime.Date).TotalMinutes))
            .Take(1)
            .ToList();

        if (!closestRideTimes.Any() && defaultToNextAvailable)
        {
            var previous = await rideTimesQuery
                .Where(rt => rt.RideDateTime <= minTime)
                .OrderByDescending(x => x.RideDateTime)
                .FirstOrDefaultAsync();
            
            var next = await rideTimesQuery
                .Where(rt => rt.RideDateTime > maxTime)
                .OrderBy(x => x.RideDateTime)
                .FirstOrDefaultAsync();

            closestRideTimes = new List<RideTime>();
            closestRideTimes.Add(previous ?? new RideTime { RideDateTime = DateTime.MinValue });
            closestRideTimes.Add(next ?? new RideTime { RideDateTime = DateTime.MaxValue });
        }
        
        return closestRideTimes;
    }

    public RideTime? GettingBestAvailableRideTime(DateTime pickUpDateAndTime,
        Guid cityId, int numberOfPassengers,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true )
    {
        var minTime = pickUpDateAndTime.AddMinutes(-60);
        var maxTime = pickUpDateAndTime.AddMinutes(60);
        var rideTimes = CreateQuery(userId, roleName)
            .Where(rt => rt.IsTaken == false)
            .Where(rt => rt.Driver!.CityId.Equals(cityId))
            .Where(rt => rt.Schedule!.Vehicle!.NumberOfSeats > numberOfPassengers)
            .Where(rt => rt.RideDateTime >= minTime && rt.RideDateTime <= maxTime)
            .ToList();
        var closestRideTime = rideTimes.OrderBy(x => Math.Abs(pickUpDateAndTime.Subtract(x.RideDateTime.Date).TotalMinutes))
            .FirstOrDefault();
            
        return closestRideTime;
    }

    public async Task<IEnumerable<RideTime>> GetAllAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking).ToListAsync();
    }

    public IEnumerable<RideTime> GetAll(Guid? userId, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking).ToList();
    }

    public async Task<RideTime?> FirstOrDefaultAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(r => r.Id.Equals(id));
    }

    public RideTime? FirstOrDefault(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking).FirstOrDefault(r => r.Id.Equals(id));
    }

    protected IQueryable<RideTime> CreateQuery(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName == null)
            return query = query.Include(c => c.Schedule)
                .ThenInclude(c => c!.Driver).ThenInclude(c => c!.AppUser)
                .Include(c => c.Schedule)
                .ThenInclude(s => s!.Vehicle)
                .ThenInclude(v => v!.VehicleMark)
                .Include(s => s.Schedule)
                .ThenInclude(s => s!.Vehicle)
                .ThenInclude(v => v!.VehicleModel);
        query = query.Include(c => c.Schedule)
            .ThenInclude(c => c!.Driver).ThenInclude(c => c!.AppUser)
            .Where(u => u.Driver!.AppUserId.Equals(userId))
            .Include(c => c.Schedule)
            .ThenInclude(s => s!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(s => s.Schedule)
            .ThenInclude(s => s!.Vehicle)
            .ThenInclude(v => v!.VehicleModel);

        return query;
    }
   
}