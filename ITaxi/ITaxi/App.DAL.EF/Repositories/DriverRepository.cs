using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverRepository: BaseEntityRepository<Driver, AppDbContext>, IDriverRepository
{
    public DriverRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }

    protected override IQueryable<Driver> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(a => a.AppUser)
            .Include(a => a.City);
        return query;
    }

    public override Task<Driver?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefaultAsync(d => d.Id.Equals(id));
    }

    public override Driver? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(d => d.Id.Equals(id));
    }

    public async Task<IEnumerable<Driver>> GetAllDriversOrderedByLastNameAsync(bool noTracking = true)
    {
        var res = await CreateQuery(noTracking).OrderBy(d => d.AppUser!.LastName)
            .ThenBy(d => d.AppUser!.FirstName).ToListAsync();

        return res;
    }

    public IEnumerable<Driver> GetAllDriversOrderedByLastName(bool noTracking = true)
    {
        var drivers = CreateQuery(noTracking).OrderBy(d => d.AppUser!.LastName);
        return drivers.ToList();
    }

    
}