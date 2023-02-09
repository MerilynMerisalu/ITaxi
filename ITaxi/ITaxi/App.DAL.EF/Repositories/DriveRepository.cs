using System.Collections;
using System.Linq.Expressions;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriveRepository : BaseEntityRepository<DriveDTO, App.Domain.Drive, AppDbContext>, IDriveRepository
{
    public DriveRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.DriveDTO, App.Domain.Drive> mapper) : 
        base(dbContext, mapper)
    {
    }

    public  async Task<IEnumerable<DriveDTO>> GetAllAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        return (await CreateQuery(userId, roleName,noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriveDTO> GetAll(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        return CreateQuery(userId,roleName,noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<DriveDTO?> FirstOrDefaultAsync(Guid id,Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName,noTracking)
            .FirstOrDefaultAsync(d => d.Id.Equals(id)));
    }

    public  DriveDTO? FirstOrDefault(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName,noTracking)
            .FirstOrDefault(d => d.Id.Equals(id)));
    }

    public async Task<IEnumerable<DriveDTO>> GetAllDrivesWithoutIncludesAsync(
        bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriveDTO> GetAllDrivesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<DriveDTO>> GettingAllOrderedDrivesWithIncludesAsync(
        Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await CreateQuery(userId, roleName, noTracking).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<DriveDTO>> GettingAllOrderedDrivesWithIncludesAsync(
        Guid? userId = null, string? roleName = null ,bool? noTracking = true)
    {
        return (await CreateQuery(userId,roleName)
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
            .ToListAsync()).Select(e => Mapper.Map(e))!;
    }


    public IEnumerable<DriveDTO> GettingAllOrderedDrivesWithIncludes(
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
            .ToList().Select(e => Mapper.Map(e))!;
    }


    public async Task<DriveDTO?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefaultAsync(d => d.Id.Equals(id)));
    }

    public DriveDTO? GetDriveWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefault(d => d.Id.Equals(id)));
    }

    public async Task<IEnumerable<DriveDTO?>> SearchByDateAsync(DateTime search,
        Guid? userId = null, string? roleName = null)
    {
        var drives = (await CreateQuery(userId, roleName)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToListAsync()).Select(e => Mapper.Map(e));
        return drives;
    }

    

    public IEnumerable<DriveDTO?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null)
    {
        var drives = CreateQuery(userId, roleName)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToList().Select(e => Mapper.Map(e));
        return drives;
    }

    public async Task<IEnumerable<DriveDTO?>> PrintAsync(
        Guid? userId = null, string? roleName = null)
    {
        
        if (roleName is "Admin")
        {
            var drives = 
                (await CreateQuery(null, roleName).ToListAsync()).Select(e => Mapper.Map(e));
            return drives;
        }

        return (await CreateQuery(userId, roleName).ToListAsync()).Select(e => Mapper.Map(e));

    }

    public IEnumerable<DriveDTO?> Print(Guid id)
    {
        var drives = CreateQuery()
            .Where(d => d.DriverId.Equals(id)).ToList().Select(e => Mapper.Map(e));
        return drives;
    }

    

    public string PickUpDateAndTimeStr(DriveDTO drive)
    {
        return drive.Booking!.PickUpDateAndTime.ToLongDateString() + " "
                                                                   + drive.Booking!.PickUpDateAndTime
                                                                       .ToShortTimeString();
    }

    public async Task<IEnumerable<DriveDTO?>> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var res = (await CreateQuery(userId, roleName, noTracking)
            .Where(d => d.Comment!.DriveId == null)
            .ToListAsync()).Select(e => Mapper.Map(e));
        return res;
    }

    public IEnumerable<DriveDTO?> GettingDrivesWithoutComment(bool noTracking = true)
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
            .ToList().Select(e => Mapper.Map(e));
        return res;
    }

    public async Task<IEnumerable<DriveDTO?>> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
#warning add a optional parameter to CreateQuery that allows the order by to be appended in that method
        var res = (await CreateQuery(userId, roleName, noTracking)
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
            .ToListAsync()).Select(e => Mapper.Map(e));
        return res;
    }

    public IEnumerable<DriveDTO?> GettingDrivesForComments(bool noTracking = true)
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
            .ToList().Select(e => Mapper.Map(e));
        return res;
    }

    public DriveDTO? AcceptingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveAccepted = true;

            return drive;
        }

        return null;
    }

    public async Task<DriveDTO?> AcceptingDriveAsync(Guid id,Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName);
        if (drive != null)
        {
            drive.IsDriveAccepted = true;
            return drive;
        }

        return null;
    }

    public DriveDTO? DecliningDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveDeclined = true;

            return drive;
        }

        return null;
    }

    public async Task<DriveDTO?> DecliningDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName,noTracking );
        if (drive != null)
        {
            drive.IsDriveDeclined = true;
            return drive;
        }

        return null;
    }

    public DriveDTO? StartingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveStarted = true;

            return drive;
        }

        return null;
    }

    public async Task<DriveDTO?> StartingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName, noTracking);
        if (drive != null)
        {
            drive.IsDriveStarted = true;
            return drive;
        }

        return null;
    }

    public DriveDTO? EndingDrive(Guid id)
    {
        var drive = FirstOrDefault(id);
        if (drive != null)
        {
            drive.IsDriveFinished = true;

            return drive;
        }

        return null;
    }

    public async Task<DriveDTO?> EndingDriveAsync(Guid id,Guid? userId = null, string? roleName = null,bool noTracking = true )
    {
        var drive = await FirstOrDefaultAsync(id, userId, roleName, noTracking);
        if (drive != null)
        {
            drive.IsDriveFinished = true;
            return drive;
        }

        return null;
    }

    public async Task<DriveDTO?> GettingFirstDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking)
            .FirstOrDefaultAsync(d => d.Id.Equals(id)));
    }

    public DriveDTO? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName, noTracking)
            .FirstOrDefault(d => d.Id.Equals(id)));
    }

    public async Task<DriveDTO?> GettingDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await RepoDbSet.FirstOrDefaultAsync(d => d.Booking!.Id.Equals(id)));
    }

    public DriveDTO? GettingDrive(Guid bookingId, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(RepoDbSet.FirstOrDefault(d => d.Booking!.Id.Equals(bookingId)));

    }

    public async Task<DriveDTO?> GettingSingleOrDefaultDriveAsync(
        Expression<Func<Drive, bool>> filter, string? roleName = null, bool noTracking = true)
    {
        var drives = CreateQuery(null, roleName);
        var drive = await drives.SingleOrDefaultAsync(filter);
        if (drive == null)
        {
            return null;
        }
        return Mapper.Map(drive);
    }

    public DriveDTO? GettingSingleOrDefaultDrive(Expression<Func<Drive, bool>> filter, string? roleName = null, bool noTracking = true)
    {
        var drives = CreateQuery(null, roleName);
        var drive = drives.SingleOrDefault(filter);
        if (drive == null)
        {
            return null;
        }
        return Mapper.Map(drive);
    }


    protected  IQueryable<Drive> CreateQuery(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

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