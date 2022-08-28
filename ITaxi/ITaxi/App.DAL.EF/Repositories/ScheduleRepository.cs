using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ScheduleRepository : BaseEntityRepository<Schedule, AppDbContext>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Schedule>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Schedule> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Schedule>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Schedule> GetAllSchedulesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }


    public async Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithIncludesAsync
        (Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var res = await CreateQuery(userId, roleName, noTracking)
            .OrderBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.StartDateAndTime.Year)
            .ThenBy(s => s.StartDateAndTime.Month)
            .ThenBy(s => s.StartDateAndTime.Day)
            .ThenBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .ThenBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.EndDateAndTime.Year)
            .ThenBy(s => s.EndDateAndTime.Month)
            .ThenBy(s => s.EndDateAndTime.Day)
            .ThenBy(s => s.EndDateAndTime.Hour)
            .ThenBy(s => s.EndDateAndTime.Minute)
            .ToListAsync();
        return res;
    }

    public IEnumerable<Schedule> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true)
    {
        return RepoDbSet
            .OrderBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.StartDateAndTime.Year)
            .ThenBy(s => s.StartDateAndTime.Month)
            .ThenBy(s => s.StartDateAndTime.Day)
            .ThenBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .ThenBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.EndDateAndTime.Year)
            .ThenBy(s => s.EndDateAndTime.Month)
            .ThenBy(s => s.EndDateAndTime.Day)
            .ThenBy(s => s.EndDateAndTime.Hour)
            .ThenBy(s => s.EndDateAndTime.Minute)
            .ToList();
    }

    public async Task<IEnumerable<Schedule>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking)
            .OrderBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.StartDateAndTime.Year)
            .ThenBy(s => s.StartDateAndTime.Month)
            .ThenBy(s => s.StartDateAndTime.Day)
            .ThenBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .ThenBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.EndDateAndTime.Year)
            .ThenBy(s => s.EndDateAndTime.Month)
            .ThenBy(s => s.EndDateAndTime.Day)
            .ThenBy(s => s.EndDateAndTime.Hour)
            .ThenBy(s => s.EndDateAndTime.Minute)
            .ToListAsync();
    }

    public IEnumerable<Schedule> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).OrderBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.StartDateAndTime.Year)
            .ThenBy(s => s.StartDateAndTime.Month)
            .ThenBy(s => s.StartDateAndTime.Day)
            .ThenBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .ThenBy(s => s.StartDateAndTime.Date)
            .ThenBy(s => s.EndDateAndTime.Year)
            .ThenBy(s => s.EndDateAndTime.Month)
            .ThenBy(s => s.EndDateAndTime.Day)
            .ThenBy(s => s.EndDateAndTime.Hour)
            .ThenBy(s => s.EndDateAndTime.Minute)
            .ToList();
    }


    public async Task<Schedule?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking)
            .FirstOrDefaultAsync(s => s.Id.Equals(id));
    }

    public Schedule? GetScheduleWithoutIncludes(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(s => s.Id.Equals(id));
    }


    public Schedule? GettingTheFirstSchedule(Guid? userId, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName)
            .OrderBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .FirstOrDefault();
    }

    public DateTime[] GettingStartAndEndTime(IEnumerable<Schedule> schedules, Guid? userId = null,
        string? roleName = null)
    {
#warning needs improving
        var scheduleStartAndEndTime = new DateTime[2];
        var schedule = schedules.First();
        if (schedule != null)
        {
            scheduleStartAndEndTime[0] = schedule.StartDateAndTime;

            scheduleStartAndEndTime[1] = schedule.EndDateAndTime;
        }


        return scheduleStartAndEndTime;
    }


    public async Task<Schedule?> FirstOrDefaultAsync(Guid id, Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(s => s.Id.Equals(id));
    }


    public async Task<Schedule?> GettingTheFirstScheduleAsync(Guid id, Guid? userid = null, string? roleName = null,
        bool noTracking = true)
    {
        var res = await CreateQuery(userid, roleName).FirstOrDefaultAsync(s => s.Id.Equals(id));
        return res;
    }

    public async Task<Guid> GettingScheduleByDriverIdAsync(Guid driverId)
    {
        return await RepoDbSet
            .Where(s => s.DriverId.Equals(driverId)).Select(s => s.Id)
            .FirstAsync();
    }

    /*public Guid GettingScheduleByDriverId(Guid driverId)
    {
        return RepoDbSet.Where(s => s.DriverId.Equals(driverId))
            .Select(s => s.Id).First();
    }*/

    protected IQueryable<Schedule> CreateQuery(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName is nameof(Admin))
        {
            query = query.Include(c => c.Driver)
                .ThenInclude(a => a!.AppUser)
                .Include(s => s.Vehicle)
                .ThenInclude(s => s!.VehicleMark)
                .Include(s => s.Vehicle)
                .ThenInclude(s => s!.VehicleModel)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v!.VehicleType);
            return query;
        }

        query = query.Include(c => c.Driver)
            .ThenInclude(a => a!.AppUser)
            .Include(s => s.Vehicle)
            .ThenInclude(s => s!.VehicleMark)
            .Include(s => s.Vehicle)
            .ThenInclude(s => s!.VehicleModel)
            .Include(v => v.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .Where(s => s.Driver!.AppUserId.Equals(userId));
        return query;
    }
}