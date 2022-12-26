using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Mappers;
using App.Domain;
using App.DTO.AdminArea;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CountyRepository : BaseEntityRepository<App.DTO.AdminArea.CountyDTO,App.Domain.County, AppDbContext>, ICountyRepository
{
    public CountyRepository(AppDbContext dbContext, IMapper<App.DTO.AdminArea.CountyDTO, App.Domain.County> mapper) :
        base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<App.DTO.AdminArea.CountyDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<App.DTO.AdminArea.CountyDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<App.DTO.AdminArea.CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(
        bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(c => c.CountyName).ToListAsync())
            .Select(e => Mapper.Map(e)!); 
    }

    public async Task<bool> HasCities(Guid countyId)
    {
        return await RepoDbContext.Cities.AnyAsync(x => x.CountyId == countyId);
    }
    public IEnumerable<App.DTO.AdminArea.CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true)
    {
        return (CreateQuery(noTracking).OrderBy(c => c.CountyName).ToList()
            .Select(e => Mapper.Map(e))!);
    }


    protected override IQueryable<App.Domain.County> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.OrderBy(c => c.CountyName).AsQueryable();
        if (noTracking) query.AsNoTracking();

        return query;
    }
}