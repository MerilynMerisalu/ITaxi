using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverRepository : BaseEntityRepository<DriverDTO, App.Domain.Driver, AppDbContext>, IDriverRepository
{
    public DriverRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.DriverDTO, App.Domain.Driver> mapper)
        : base(dbContext, mapper)
    {
    }

    public override async Task<DriverDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes).FirstOrDefaultAsync(d => d.Id.Equals(id)));
    }

    public override DriverDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(d => d.Id.Equals(id)));
    }

    public async Task<IEnumerable<DriverDTO>> GetAllDriversOrderedByLastNameAsync(bool noTracking = true)
    {
        var res = await CreateQuery(noTracking).OrderBy(d => d.AppUser!.LastName)
            .ThenBy(d => d.AppUser!.FirstName).ToListAsync();

        return res.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriverDTO> GetAllDriversOrderedByLastName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(d => d.AppUser!.LastName)
            .ThenBy(d => d.AppUser!.FirstName)
            .ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<DriverDTO> GettingDriverByVehicleAsync(Guid vehicleId, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes)
            .SingleOrDefaultAsync(d => d.Vehicles!.Any(v => v.Id.Equals(vehicleId))))!;
    }

    public async Task<DriverDTO> GettingDriverByAppUserIdAsync(Guid driverAppUserId, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes)
            .SingleOrDefaultAsync(d => d.AppUserId.Equals(driverAppUserId)))!;
    }

    public async Task<bool> HasAnySchedulesAsync(Guid driverId, bool noTracking = true)
    {
        return await RepoDbContext.Schedules.AnyAsync(s => s.DriverId.Equals(driverId));
    }

    public bool HasAnySchedules(Guid driverId, bool noTracking = true)
    {
        return RepoDbContext.Schedules.Any(s => s.DriverId.Equals(driverId));
    }

    public async Task<bool> HasAnyBookingsAsync(Guid driverId, bool noTracking = true)
    {
        return await RepoDbContext.Bookings.AnyAsync(b => b.DriverId.Equals(driverId));
    }

    public bool HasAnyBookings(Guid driverId, bool noTracking = true)
    {
        return RepoDbContext.Bookings.Any(b => b.DriverId.Equals(driverId));
    }

    public async Task<bool> HasAnyDriversAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .AnyAsync(d => d.DriverLicenseCategories!.Any(dl => dl.DriverId.Equals(id)));
    }


    protected override IQueryable<Driver> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(a => a.AppUser)
                         .Include(a => a.City);
        return query;
    }
}