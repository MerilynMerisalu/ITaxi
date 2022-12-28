using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CountyRepository : BaseEntityRepository<CountyDTO, App.Domain.County, AppDbContext>, ICountyRepository
{
    public CountyRepository(AppDbContext dbContext, IMapper<CountyDTO, App.Domain.County> mapper) :
        base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<CountyDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<CountyDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(
        bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(c => c.CountyName).ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> HasCities(Guid countyId)
    {
        return await RepoDbContext.Cities.AnyAsync(x => x.CountyId == countyId);
    }
    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true)
    {
        return (CreateQuery(noTracking).OrderBy(c => c.CountyName).ToList()
            .Select(e => Mapper.Map(e))!);
    }


    protected override IQueryable<App.Domain.County> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.OrderBy(c => c.CountyName).Include(x => x.Cities).AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        return query;
    }
}