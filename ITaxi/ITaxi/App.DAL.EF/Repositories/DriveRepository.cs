using System.Collections;
using System.Linq.Expressions;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriveRepository : BaseEntityRepository<Drive, AppDbContext>, IDriveRepository
{
    public DriveRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public  async Task<IEnumerable<Drive>> GetAllAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking).ToListAsync();
    }

    public IEnumerable<Drive> GetAll(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        return CreateQuery(userId,roleName,noTracking).ToList();
    }

    public async Task<Drive?> FirstOrDefaultAsync(Guid id,Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id));
    }

    public  Drive? FirstOrDefault(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<IEnumerable<Drive>> GetAllDrivesWithoutIncludesAsync(
        bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Drive> GetAllDrivesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Drive>> GettingAllOrderedDrivesWithIncludesAsync(
        Guid? userId = null, string? roleName = null ,bool? noTracking = true)
    {
        return await CreateQuery(userId,roleName)
            .OrderBy(d => d.Booking!.PickUpDateAndTime.Date)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Day)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Month)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Year)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.LastName)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.FirstName)
            .ThenBy(d => d.Booking!.City!.CityName)
            .ThenBy(d => d.Booking!.PickupAddress)
            .ThenBy(d => d.Booking!.DestinationAddress)
            .ThenBy(d => d.Booking!.VehicleType!.VehicleTypeName)
            .ToListAsync();
    }


    public IEnumerable<Drive> GettingAllOrderedDrivesWithIncludes(
        Guid? userId = null, string? roleName = null, bool noTracking = true )
    {
        return CreateQuery(userId, roleName,noTracking)
            .OrderBy(d => d.Booking!.PickUpDateAndTime.Date)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Day)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Month)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Year)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.LastName)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.FirstName)
            .ThenBy(d => d.Booking!.City!.CityName)
            .ThenBy(d => d.Booking!.PickupAddress)
            .ThenBy(d => d.Booking!.DestinationAddress)
            .ThenBy(d => d.Booking!.VehicleType!.VehicleTypeName)
            .ToList();
    }


    public async Task<Drive?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id));
    }

    public Drive? GetDriveWithoutIncludes(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<IEnumerable<Drive?>> SearchByDateAsync(DateTime search,
        Guid? userId = null, string? roleName = null)
    {
        var drives = await CreateQuery(userId, roleName)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToListAsync();
        return drives;
    }

    

    public IEnumerable<Drive?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null)
    {
        var drives = CreateQuery(userId, roleName)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToList();
        return drives;
    }

    public async Task<IEnumerable<Drive?>> PrintAsync(
        Guid? userId = null, string? roleName = null)
    {
        
        if (roleName is nameof(Admin))
        {
            var drives = await CreateQuery(null, roleName).ToListAsync();
            return drives;
        }

        return await CreateQuery(userId, roleName).ToListAsync();

    }

    public IEnumerable<Drive?> Print(Guid id)
    {
        var drives = CreateQuery()
            .Where(d => d.DriverId.Equals(id)).ToList();
        return drives;
    }

    public string PickUpDateAndTimeStr(Drive drive)
    {
        return drive.Booking!.PickUpDateAndTime.ToLongDateString() + " "
                                                                   + drive.Booking!.PickUpDateAndTime
                                                                       .ToShortTimeString();
    }

    public async Task<IEnumerable<Drive?>> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var res = await CreateQuery(userId, roleName, noTracking)
            .Where(d => d.Comment!.DriveId == null)
            .ToListAsync();
        return res;
    }

    public IEnumerable<Drive?> GettingDrivesWithoutComment(bool noTracking = true)
    {
        var res = CreateQuery(noTracking)
            .OrderBy(d => d.Booking!.PickUpDateAndTime.Date)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Day)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Month)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Year)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.LastName)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.FirstName)
            .ThenBy(d => d.Booking!.City!.CityName)
            .ThenBy(d => d.Booking!.PickupAddress)
            .ThenBy(d => d.Booking!.DestinationAddress)
            .ThenBy(d => d.Booking!.VehicleType!.VehicleTypeName)
            .Where(d => d.Comment!.DriveId == null)
            .ToList();
        return res;
    }

    public async Task<IEnumerable<Drive?>> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
#warning add a optional parameter to CreateQuery that allows the order by to be appended in that method
        var res = await CreateQuery(userId, roleName, noTracking)
            .OrderBy(d => d.Booking!.PickUpDateAndTime.Date)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Day)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Month)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Year)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.LastName)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.FirstName)
            .ThenBy(d => d.Booking!.City!.CityName)
            .ThenBy(d => d.Booking!.PickupAddress)
            .ThenBy(d => d.Booking!.DestinationAddress)
            .ThenBy(d => d.Booking!.VehicleType!.VehicleTypeName)
            .ToListAsync();
        return res;
    }

    public IEnumerable<Drive?> GettingDrivesForComments(bool noTracking = true)
    {
        var res = CreateQuery(noTracking)
            .OrderBy(d => d.Booking!.PickUpDateAndTime.Date)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Day)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Month)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Year)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(d => d.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.LastName)
            .ThenBy(d => d.Booking!.Customer!.AppUser!.FirstName)
            .ThenBy(d => d.Booking!.City!.CityName)
            .ThenBy(d => d.Booking!.PickupAddress)
            .ThenBy(d => d.Booking!.DestinationAddress)
            .ThenBy(d => d.Booking!.VehicleType!.VehicleTypeName)
            .ToList();
        return res;
    }

    public Drive? AcceptingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveAccepted = true;

            return drive;
        }

        return null;
    }

    public async Task<Drive?> AcceptingDriveAsync(Guid id,Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName);
        if (drive != null)
        {
            drive.IsDriveAccepted = true;
            return drive;
        }

        return null;
    }

    public Drive? DecliningDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveDeclined = true;

            return drive;
        }

        return null;
    }

    public async Task<Drive?> DecliningDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName,noTracking );
        if (drive != null)
        {
            drive.IsDriveDeclined = true;
            return drive;
        }

        return null;
    }

    public Drive? StartingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveStarted = true;

            return drive;
        }

        return null;
    }

    public async Task<Drive?> StartingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName, noTracking);
        if (drive != null)
        {
            drive.IsDriveStarted = true;
            return drive;
        }

        return null;
    }

    public Drive? EndingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveFinished = true;

            return drive;
        }

        return null;
    }

    public async Task<Drive?> EndingDriveAsync(Guid id,Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName, noTracking);
        if (drive != null)
        {
            drive.IsDriveFinished = true;
            return drive;
        }

        return null;
    }

    public async Task<Drive?> GettingFirstDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id));
    }

    public Drive? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<Drive?> GettingSingleOrDefaultDriveAsync(
        Expression<Func<Drive, bool>> filter, string? roleName = null, bool noTracking = true)
    {
        var drives = CreateQuery(null, roleName);
        var drive = await drives.SingleOrDefaultAsync(filter);
        if (drive == null)
        {
            return null;
        }
        return drive;
    }

    public Drive? GettingSingleOrDefaultDrive(Expression<Func<Drive, bool>> filter, string? roleName = null, bool noTracking = true)
    {
        var drives = CreateQuery(null, roleName);
        var drive = drives.SingleOrDefault(filter);
        if (drive == null)
        {
            return null;
        }
        return drive;
    }


    protected  IQueryable<Drive> CreateQuery(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName is nameof(Admin))
        {
           return query.Include(d => d.Booking)
               .ThenInclude(d => d!.Schedule)
               .Include(c => c.Booking)
               .ThenInclude(c => c!.Customer)
               .ThenInclude(c => c!.AppUser)
               .Include(c => c.Booking)
               .ThenInclude(c => c!.Customer)
               .ThenInclude(c => c!.DisabilityType)
               .ThenInclude(c => c!.DisabilityTypeName)
               .ThenInclude(c => c.Translations)
               .Include(c => c.Booking)
               .ThenInclude(c => c!.City)
               .Include(b => b.Booking)
               .Include(v => v.Booking)
               .ThenInclude(v => v!.Vehicle)
               .ThenInclude(v => v!.VehicleType)
               .ThenInclude(c => c!.VehicleTypeName)
               .ThenInclude(c => c.Translations)
               .Include(v => v.Booking)
               .ThenInclude(v => v!.Vehicle)
               .ThenInclude(v => v!.VehicleMark)
               .Include(v => v.Booking)
               .ThenInclude(v => v!.Vehicle)
               .ThenInclude(v => v!.VehicleModel)
               .Include(c => c.Comment)
               .Include(d => d.Driver)
               .ThenInclude(d => d!.AppUser);
        }
        query = query.Include(d => d.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.DisabilityType)
            .ThenInclude(c => c!.DisabilityTypeName)
            .ThenInclude(c => c.Translations)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(b => b.Booking)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .ThenInclude(c => c!.VehicleTypeName)
            .ThenInclude(c => c.Translations)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(c => c.Comment)
            .Include(d => d.Driver)
            .ThenInclude(d => d!.AppUser)
            .Where(d => d.Driver!.AppUserId.Equals(userId) 
                        || d.Booking!.Customer!.AppUserId.Equals(userId));
        return query;
    }

    
}