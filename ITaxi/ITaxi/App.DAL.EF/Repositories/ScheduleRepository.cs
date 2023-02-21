using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ScheduleRepository : BaseEntityRepository<ScheduleDTO, App.Domain.Schedule, AppDbContext>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.ScheduleDTO, App.Domain.Schedule> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<ScheduleDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<ScheduleDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<ScheduleDTO>> GetAllSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GetAllSchedulesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e))!;
    }


    public async Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithIncludesAsync
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
        return res.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithIncludes(bool noTracking = true)
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
            .ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<ScheduleDTO>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true)
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
            .ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingAllOrderedSchedulesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).OrderBy(s => s.StartDateAndTime.Date)
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
            .ToList().Select(e => Mapper.Map(e))!;
    }


    public async Task<ScheduleDTO?> GettingScheduleWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefaultAsync(s => s.Id.Equals(id)));
    }

    public ScheduleDTO? GetScheduleWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes: true).FirstOrDefault(s => s.Id.Equals(id)));
    }


    public ScheduleDTO? GettingTheFirstScheduleById(Guid id, Guid? userId, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName)
            .OrderBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .FirstOrDefault(s => s.Id.Equals(id)));
    }

    public async Task<ScheduleDTO?> GettingTheFirstScheduleAsync(Guid? userid = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userid, roleName)
            .OrderBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .FirstOrDefaultAsync());
    }

    public ScheduleDTO? GettingTheFirstSchedule(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName)
            .OrderBy(s => s.StartDateAndTime.Hour)
            .ThenBy(s => s.StartDateAndTime.Minute)
            .FirstOrDefault());
    }


    /// <summary>
    /// Select the Schedules for the specified <paramref name="driverId"/>
    /// </summary>
    /// <param name="driverId">The Driver to filter the Schedules to</param>
    /// <param name="userId">The current user context</param>
    /// <param name="roleName">The current User's Role</param>
    /// <param name="noTracking">Flag to disable tracking of the returned entities</param>
    /// <returns>List of Schedules with the default includes</returns>
    public async Task<IEnumerable<ScheduleDTO>> GettingTheScheduleByDriverIdAsync(Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await CreateQuery(userId, roleName, noTracking)
            .Where(x => x.DriverId == driverId)
            .OrderBy(x => x.StartDateAndTime)
            .ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<ScheduleDTO> GettingTheScheduleByDriverId(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking)
            .Where(x => x.DriverId == driverId)
            .OrderBy(x => x.StartDateAndTime)
            .ToList().Select(e => Mapper.Map(e))!;
    }


    /*public static DateTime[] GettingStartAndEndTime(Schedule[] schedules, Guid? userId = null,
        string? roleName = null)
    {
#warning needs improving
        var scheduleStartAndEndTime = new DateTime[2];
        var schedule = schedules.FirstOrDefault();
        if (schedule != null)
        {
            scheduleStartAndEndTime[0] = schedule.StartDateAndTime;

            scheduleStartAndEndTime[1] = schedule.EndDateAndTime;
        }*/


        // return scheduleStartAndEndTime;
    // }

    public int NumberOfRideTimes(Guid? driverId = null, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var numberOfRideTimes =
            CreateQuery(userId, roleName, noTracking).Where(d => d.DriverId.Equals(driverId)).Select(rt => rt.RideTimes).Count();
        return numberOfRideTimes;
    }

    public int? NumberOfTakenRideTimes(Guid? driverId = null, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var numberOfRideTimesTaken = CreateQuery(userId, roleName, noTracking)
            .Select(d => d.RideTimes!.Where(rt => rt.IsTaken == true)).Count();
        if (numberOfRideTimesTaken == null)
        {
            numberOfRideTimesTaken = 0;
            return numberOfRideTimesTaken;
        }
        return numberOfRideTimesTaken;
    }

    public async Task<bool> HasAnyScheduleAsync(Guid id, bool noTracking = true)
    {
        return await RepoDbContext.Bookings.AnyAsync(b => b.ScheduleId.Equals(id));
    }

    public bool HasAnySchedule(Guid id, bool noTracking = true)
    {
        return RepoDbContext.Bookings.Any(b => b.ScheduleId.Equals(id));
    }


    public async Task<ScheduleDTO?> FirstOrDefaultAsync(Guid id, Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(s => s.Id.Equals(id)));
    }


    public async Task<ScheduleDTO?> GettingTheFirstScheduleByIdAsync(Guid id, Guid? userid = null, string? roleName = null,
        bool noTracking = true)
    {
        var res = await CreateQuery(userid, roleName).FirstOrDefaultAsync(s => s.Id.Equals(id));
        return Mapper.Map(res);
    }



    protected IQueryable<Schedule> CreateQuery(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        if (roleName == null)
        {
            query = query.Include(c => c.Driver)
                .ThenInclude(a => a!.AppUser)
                .Include(s => s.Vehicle)
                .ThenInclude(s => s!.VehicleMark)
                .Include(s => s.Vehicle)
                .ThenInclude(s => s!.VehicleModel)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v!.VehicleType)
                .ThenInclude(v => v!.VehicleTypeName)
                .ThenInclude(v => v.Translations)
                .Include(s => s.RideTimes);
                
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
            .ThenInclude(v => v!.VehicleTypeName)
            .ThenInclude(v => v.Translations)
            .Include(s => s.RideTimes)
            .Where(s => s.Driver!.AppUserId.Equals(userId));
        return query;
    }
}