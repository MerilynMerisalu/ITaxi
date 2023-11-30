using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;


public class RideTimeRepository : BaseEntityRepository<App.DAL.DTO.AdminArea.RideTimeDTO,
    App.Domain.RideTime, AppDbContext>, IRideTimeRepository
{
    public RideTimeRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.AdminArea.RideTimeDTO, App.Domain.RideTime> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<RideTimeDTO?> GettingAllRideTimesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e));
    }

    public async Task<RideTimeDTO?> GettingRideTimeWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true).FirstOrDefaultAsync(r => r.ScheduleId.Equals(id)));
    }

    public RideTimeDTO? GettingRideTimeWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(base.CreateQuery(noTracking, noIncludes: true).FirstOrDefault(r => r.ScheduleId.Equals(id)));
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true)//, bool noIncludes = false)
    {
        return (await CreateQuery(userId, roleName, noTracking, noIncludes:false)
            .OrderBy(r => r.Schedule!.StartDateAndTime)
            .ThenBy(r => r.Schedule!.EndDateAndTime)
            .ThenBy(r => r.RideDateTime)
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimes(Guid? userId, string? roleName = null,
        bool noTracking = true)//, bool noIncludes = false)
    {
        return CreateQuery(userId, roleName, noTracking, noIncludes: false)
            .OrderBy(r => r.Schedule!.StartDateAndTime)
            .ThenBy(r => r.Schedule!.EndDateAndTime)
            .ThenBy(r => r.RideDateTime)
            .ToList().Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<RideTimeDTO?>> GettingAllOrderedRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking, noIncludes: true)
            .OrderBy(r => r.RideDateTime)
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<RideTimeDTO?> GettingAllOrderedRideTimesWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking, noIncludes: true)
            .OrderBy(r => r.RideDateTime)
            .ToList().Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<string?>> GettingAllSelectedRideTimesAsync(RideTimeDTO rideTime,
        bool noTracking = true)
    {
        return await CreateQuery()
            .Where(r => r.ScheduleId.Equals(rideTime.ScheduleId))
            .Select(r => r.RideDateTime.ToString("t"))
            .ToListAsync();
    }

    public IEnumerable<string?> GettingAllSelectedRideTimes(RideTimeDTO rideTime, bool noTracking = true)
    {
        return CreateQuery()
            .Where(r => r.ScheduleId.Equals(rideTime.ScheduleId))
            .Select(r => r.RideDateTime.ToString("t"))
            .ToList();
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking, noIncludes)
            .FirstOrDefaultAsync(r => r.Id.Equals(id)));
    }

    public RideTimeDTO? GettingFirstRideTimeById(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(userId, roleName, noTracking, noIncludes).FirstOrDefault(r => r.Id.Equals(id)));
    }

    public async Task<RideTimeDTO?> GettingFirstRideTimeByBookingIdAsync(Guid id, Guid? userId = null,
        string? roleName = null,
        bool noTracking = true, bool noIncludes = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking, noIncludes)
            .FirstOrDefaultAsync(r => r.BookingId.Equals(id)));
    }

    public RideTimeDTO? GettingFirstRideTimeByBookingId(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName, noTracking, noIncludes).FirstOrDefault(r => r.BookingId.Equals(id)));
    }

    

    public  List<string> CalculatingRideTimes(Guid id)
    {
        var schedule = RepoDbContext.Schedules.FirstOrDefault(s => s.Id.Equals(id));
        var times = new List<string>();
        var start = schedule!.StartDateAndTime.ToLocalTime();
        var time = start;
        var end = schedule.EndDateAndTime.ToLocalTime();

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

    public async Task<IList<RideTimeDTO>> GettingBestAvailableRideTimeAsync(DateTime pickUpDateAndTime,
        Guid cityId,
        int numberOfPassengers,
        Guid vehicleType,
        bool defaultToNextAvailable,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        // Change this to 5 to make the match time +- 5minutes of a ride time
        int closestTimeRangeMinutes = 1;
        var minTime = pickUpDateAndTime.AddMinutes(-closestTimeRangeMinutes);
        var maxTime = pickUpDateAndTime.AddMinutes(closestTimeRangeMinutes);
        var utcNow = DateTime.UtcNow;
        var timeMinusOne = pickUpDateAndTime.AddMinutes(-1);
        var timePlusOne = pickUpDateAndTime.AddMinutes(1);
        var rideTimesQuery = CreateQuery(userId, roleName, false) // overwrite the user, we need tracking
            .Where(rt => rt.IsTaken == false)
            .Where(rt => rt.ExpiryTime == null || rt.ExpiryTime < utcNow)
            .Where(rt => rt.Schedule!.Driver!.CityId.Equals(cityId))
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
            // we can respect the caller's no tracking now
            if (noTracking)
                rideTimesQuery = rideTimesQuery.AsNoTracking();

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
        else if (closestRideTimes.Count == 1)
        {
            // we need to set the expiry time X Minutes into the future, so that this RideTime is not 
            // available to other operators, but it will automatically become available even if the 
            // user's session  becomes dormont
            closestRideTimes[0].ExpiryTime = utcNow.AddMinutes(9);
            await RepoDbContext.SaveChangesAsync();
        }

        return closestRideTimes.Select(e => Mapper.Map(e)).ToList()!;
    }

    public RideTimeDTO? GettingBestAvailableRideTime(DateTime pickUpDateAndTime,
        Guid cityId, int numberOfPassengers,
        Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        var minTime = pickUpDateAndTime.AddMinutes(-60);
        var maxTime = pickUpDateAndTime.AddMinutes(60);
        var rideTimes = CreateQuery(userId, roleName)
            .Where(rt => rt.IsTaken == false)
            .Where(rt => rt.Schedule!.Driver!.CityId.Equals(cityId))
            .Where(rt => rt.Schedule!.Vehicle!.NumberOfSeats > numberOfPassengers)
            .Where(rt => rt.RideDateTime >= minTime && rt.RideDateTime <= maxTime)
            .ToList();
        var closestRideTime = rideTimes.OrderBy(x => Math.Abs(pickUpDateAndTime.Subtract(x.RideDateTime.Date).TotalMinutes))
            .FirstOrDefault();

        return Mapper.Map(closestRideTime);
    }

    public List<string> GettingRemainingRideTimesByScheduleId(Guid scheduleId)
    {
        var currentSchedule = this.RepoDbContext.Schedules.Include(s => s.RideTimes).AsNoTracking().Single(x => x.Id == scheduleId);
        currentSchedule.StartDateAndTime = currentSchedule.StartDateAndTime.ToLocalTime();
        currentSchedule.EndDateAndTime = currentSchedule.EndDateAndTime.ToLocalTime();
        
        // Select the RideTimes form the currently selected schedule, for the current driver
        var rideTimesList =
            this.CalculatingRideTimes(currentSchedule.Id);//(new Schedule [] {currentSchedule}));

        // Need to remove the times that have already been issued:
        if (currentSchedule.RideTimes!.Any())
        {
            foreach (var time in currentSchedule.RideTimes!)
            {
                rideTimesList.Remove(time.RideDateTime.ToLocalTime().ToString("t"));
            }
        }

        return rideTimesList;
    }

    public async Task<bool> HasScheduleAnyAsync(Guid id, bool noTracking = true)
    {
        return await RepoDbContext.RideTimes.AnyAsync(r => r.ScheduleId.Equals(id));
    }

    public bool HasScheduleAny(Guid id, bool noTracking = true)
    {
        return RepoDbContext.RideTimes.Any(r => r.ScheduleId.Equals(id));
    }

    public async Task<IEnumerable<RideTimeDTO>> GetAllAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return (await CreateQuery(userId, roleName, noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<RideTimeDTO> GetAll(Guid? userId, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<RideTimeDTO?> FirstOrDefaultAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(r => r.Id.Equals(id)));
    }

    public RideTimeDTO? FirstOrDefault(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName, noTracking).FirstOrDefault(r => r.Id.Equals(id)));
    }

    protected IQueryable<RideTime> CreateQuery(Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        var query = CreateQuery(noTracking, noIncludes, showDeleted);
        if (noTracking) query = query.AsNoTracking();
        if (noIncludes)
        {
            return query;
        }

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

            .Include(c => c.Schedule)
            .ThenInclude(s => s!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(s => s.Schedule)
            .ThenInclude(s => s!.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Where(u => u.Schedule!.Driver!.AppUserId.Equals(userId));


        return query;
    }

}

