using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RideTimeRepository: BaseEntityRepository<RideTime, AppDbContext>, IRideTimeRepository
{
    public RideTimeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<RideTime> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.Schedule);
            
        return query;
    }

    public override async Task<IEnumerable<RideTime>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<RideTime> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override async Task<RideTime?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(r => r.Id.Equals(id));
    }

    public override RideTime? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(r => r.Id.Equals(id));
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

    public async Task<IEnumerable<RideTime?>> GettingAllOrderedRideTimesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking)
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

    public IEnumerable<RideTime?> GettingAllOrderedRideTimes(bool noTracking = true)
    {
        return CreateQuery(noTracking)
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

    public List<string> CalculatingRideTimes(DateTime[] scheduleStartAndEndTime)
    {
        List<string> times = new List<string>();
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

    public string DriveTimeFormatting(RideTime rideTime)
    {
        return rideTime.RideDateTime.ToString("t");
    }
}