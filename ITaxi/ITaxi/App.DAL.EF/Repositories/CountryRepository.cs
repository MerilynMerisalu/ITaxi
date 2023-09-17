using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CountryRepository: BaseEntityRepository<CountryDTO, Country, AppDbContext>, ICountryRepository
{
    public CountryRepository(AppDbContext dbContext, IMapper<CountryDTO, Country> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(c => Mapper.Map(c))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(bool noTracking = true)
    {
        return CreateQuery(noTracking).Select(c => Mapper.Map(c))!;
    }

    public async Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true)
    {
        return await RepoDbContext.Counties.AnyAsync(c => c.CountryId.Equals(id));
    }

    public bool HasAnyCounties(Guid id, bool noTracking = true)
    {
        return RepoDbContext.Counties.Any(c => c.CountryId.Equals(id));
    }

    protected override IQueryable<Country> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        if (noTracking)
        {
            return RepoDbSet
                .Include(c => c.CountryName)
                .ThenInclude(c => c.Translations).Where(c => c.IsDeleted == false )
                .AsNoTracking();
        }

        if (noIncludes)
        {
            return RepoDbSet;

        }

        return RepoDbSet
            .Include(c => c.CountryName)
            .ThenInclude(c => c.Translations)
            .AsNoTracking();
        
    }

    public override async Task<CountryDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return (Mapper.Map(await CreateQuery(noTracking, noIncludes)
            .FirstOrDefaultAsync(c => c.Id.Equals(id))));
    }

    public override CountryDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(c => c.Id.Equals(id)));
    }
}