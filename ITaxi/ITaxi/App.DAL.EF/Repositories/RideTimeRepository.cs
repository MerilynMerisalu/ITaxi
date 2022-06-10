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

    public async Task<IEnumerable<RideTime>> GettingAllRideTimesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<RideTime> GettingAllRideTimesWithoutIncludes(bool noTracking = true)
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
}