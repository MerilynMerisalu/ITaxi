using System.Linq.Expressions;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CityRepository: BaseEntityRepository<City, AppDbContext>, ICityRepository
{
    public CityRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    

    protected override IQueryable<City> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.County);
        return query;
    }
    
    
    public virtual IQueryable<City> CreateOrderedByCityNameQuery(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(c => c.CityName);
    }

    #warning Rewrite later to use DTO for API request
    public virtual async Task<IEnumerable<City>> GetAllCitiesWithoutCountyAsync()
    {
        var res = await base.CreateQuery().ToListAsync();
        return res;

    }

    #warning Rewrite later to use DTO for API request
    public async Task<City?> FirstOrDefaultCityWithoutCountyAsync(Guid id)
    {
        return await base.CreateQuery().FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

#warning Ask if this code should be rewritten because it doesn't seem to follow DRY code principle
    public override async Task<City?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        query = query.Include(c => c.County);

        var res = await query.FirstOrDefaultAsync(c => c.Id == id);

        return res;

    }
    
     
    public override City? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery().FirstOrDefault(c => c.Id.Equals(id));
    }

    public override Task<City?> SingleOrDefaultAsync(Expression<Func<City?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery().SingleOrDefaultAsync(c => c.Id.Equals(filter));
    }

    public override City? SingleOrDefault(Expression<Func<City?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery().SingleOrDefault(e => e.Id.Equals(filter));
    }

   
}