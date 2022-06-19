using System.Collections;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriveRepository: BaseEntityRepository<Drive, AppDbContext>, IDriveRepository
{
    public DriveRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Drive> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(d => d.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(b => b.Booking)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(c => c.Comment);
        return query;
    }

    public override async Task<IEnumerable<Drive>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Drive> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override async Task<Drive?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id));
    }

    public override Drive? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<IEnumerable<Drive>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Drive> GetAllDrivesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Drive>> GettingAllOrderedDrivesWithIncludesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking)
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


    public IEnumerable<Drive> GettingAllOrderedDrivesWithIncludes(bool noTracking = true)
    {
        return  CreateQuery(noTracking)
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
        return  base.CreateQuery(noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<IEnumerable<Drive?>> SearchByDateAsync(DateTime search)
    {
        var drives = await RepoDbSet
            .Include(b => b.Booking)
            .ThenInclude(b => b!.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(b => b.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(b => b.Booking)
            .ThenInclude(v => v!.Vehicle)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.VehicleType)
            .Include(d => d.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(c => c.Comment)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToListAsync();
        return drives;
    }

    public IEnumerable<Drive?> SearchByDate(DateTime search)
    {
        var drives =  RepoDbSet
            .Include(b => b.Booking)
            .ThenInclude(b => b!.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(b => b.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(b => b.Booking)
            .ThenInclude(v => v!.Vehicle)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(v => v.Booking)
            .ThenInclude(v => v!.VehicleType)
            .Include(d => d.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(c => c.Comment)
            .Where(d => d.Booking!.PickUpDateAndTime.Date
                .Equals(search.Date)).ToList();
        return drives;
    }

    public async Task<IEnumerable<Drive?>> PrintAsync(Guid id)
    {
        
        var drives = await RepoDbSet.Include(d => d.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(d => d.Booking)
            .ThenInclude(d => d!.VehicleType)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(d => d.Booking)
            .ThenInclude(d => d!.Driver)
            .ThenInclude(d => d!.AppUser)
            .Where(d => d.DriverId.Equals(id)).ToListAsync();
        return drives;
    }

    public IEnumerable<Drive?> Print(Guid id)
    {
        var drives = RepoDbSet.Include(d => d.Booking)
            .ThenInclude(d => d!.Schedule)
            .Include(d => d.Booking)
            .ThenInclude(d => d!.VehicleType)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.Customer)
            .ThenInclude(c => c!.AppUser)
            .Include(c => c.Booking)
            .ThenInclude(c => c!.City)
            .Include(d => d.Booking)
            .ThenInclude(d => d!.Driver)
            .ThenInclude(d => d!.AppUser)
            .Where(d => d.DriverId.Equals(id)).ToList();
        return drives;
    }

    public string PickUpDateAndTimeStr(Drive drive)
    {
        return drive.Booking!.PickUpDateAndTime.ToLongDateString() + " "
            + drive.Booking!.PickUpDateAndTime.ToShortTimeString();
    }

    public async Task<IList> GettingDrivesWithoutCommentAsync(bool noTracking = true)
    {
        var res = await  CreateQuery(noTracking)
            .Where(d =>  d.Comment.DriveId == null)
            .Select(d => new {d.Booking.PickUpDateAndTime, d.Id, d.Comment.DriveId }).ToListAsync();
        return res;
    }

    public ArrayList GettingDrivesWithoutComment(bool noTracking = true)
    {
        var res = CreateQuery().Include(d => d.Booking)
            .Where(d => d.Comment!.DriveId == null)
            .Select(d => new {d.Booking!.PickUpDateAndTime, d.Id}).ToArray();
        return new ArrayList {res};

    }
}