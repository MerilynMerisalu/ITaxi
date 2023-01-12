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

    public override async Task<DriverDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id)));
    }

    public override DriverDTO? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(d => d.Id.Equals(id)));
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

    public async Task<DriverDTO> GettingDriverByVehicleAsync(Guid driverAppUserId, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking)
            .SingleOrDefaultAsync(d => d.AppUserId.Equals(driverAppUserId)))!;
    }

    public async Task<bool> HasAnyDriversAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .AnyAsync(d => d.DriverLicenseCategories!.Any(dl => dl.DriverId.Equals(id)));
    }


    protected override IQueryable<Driver> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        query = query.Include(a => a.AppUser)
            .Include(a => a.City);
        return query;
    }
}