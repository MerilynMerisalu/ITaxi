using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CountyRepository: BaseEntityRepository<County, AppDbContext>, ICountyRepository
{
    public CountyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    protected override IQueryable<County> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.OrderBy(c => c.CountyName).AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        return query;
    }

    public override async Task<IEnumerable<County>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<County> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<County>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(c => c.CountyName).ToListAsync();
    }

    public IEnumerable<County> GetAllCountiesOrderedByCountyName(bool noTracking = true)
    {
       return CreateQuery(noTracking).OrderBy(c => c.CountyName).ToList();
    }
}