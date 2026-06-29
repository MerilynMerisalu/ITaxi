using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CountryRepository: BaseEntityRepository<CountryDTO, Country, AppDbContext>, ICountryRepository
{
    public CountryRepository(AppDbContext dbContext, IMapper<CountryDTO, Country> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true,
        bool noIncludes = false, bool showIgnored = false)
    {
        return (await CreateQuery(noTracking: noTracking, noIncludes: noIncludes, showIgnored: showIgnored).OrderBy(c => c.CountryName).ToListAsync()).Select(c => Mapper.Map(c))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(bool noTracking = true,
    bool noIncludes = false, bool showIgnored = false, bool showDeleted = false )
    {
        var result = CreateQuery(noTracking: noTracking, noIncludes: noIncludes, showIgnored: showIgnored, showDeleted: showDeleted)
            .ToList();
        return result.OrderBy(c => (string) c.CountryName).Select(c => Mapper.Map(c))!;
        
    }

    public async Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true)
    {
        return await RepoDbContext.Counties.AnyAsync(c => c.CountryId.Equals(id));
    }

    public bool HasAnyCounties(Guid id, bool noTracking = true)
    {
        return RepoDbContext.Counties.Any(c => c.CountryId.Equals(id));
    }

    

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true)
    {
        // special handling of OrderBy to account for language transalation
        return (await CreateQuery(noTracking, showDeleted: showDeleted, showIgnored: showIgnored)
            .ToListAsync()) // Bring into memory "Materialize"
            .OrderBy(v => v.ISOCode)
            .ToList().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        // special handling of OrderBy to account for language transalation
        return CreateQuery(noTracking, showIgnored: showIgnored, showDeleted: showDeleted)
            .ToList() // Bring into memory "Materialize"
            .OrderBy(v => v.ISOCode)
            
            .ToList().Select(e => Mapper.Map(e))!;
    }

    protected override IQueryable<Country> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true)
    {
        if (!showIgnored && !showDeleted)
        {
            return RepoDbSet
                .Include(c => c.CountryName)
                .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == false)
                .AsNoTracking();
        }
        if (!showIgnored) 
        {

            return RepoDbSet
                .Include(c => c.CountryName)
                .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == true)
                .AsNoTracking();
        }
        if (noTracking)
        {
            return RepoDbSet
                .Include(c => c.CountryName)
                .ThenInclude(c => c.Translations).Where(c => c.IsDeleted == false)
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

    public override async Task<CountryDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var country = (await CreateQuery(noTracking, noIncludes, showIgnored: showIgnored, showDeleted: showDeleted)
            .FirstOrDefaultAsync(c => c.Id.Equals(id)));
        return Mapper.Map(country);
    }

    public override CountryDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes, showIgnored: showIgnored, showDeleted: showDeleted).FirstOrDefault(c => c.Id.Equals(id)));
    }

    public async Task<CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes, showDeleted).FirstOrDefault(c => c.ISOCode.Equals(isoCode)));
    }

    
    public async Task<CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes, showDeleted, showIgnored)
            .FirstOrDefaultAsync(c => c.ISOCode.Equals(isoCode)));
    }

    public CountryDTO? GetCountryByISOCode(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true)
    {
       return Mapper.Map(CreateQuery(noTracking, noIncludes, showDeleted, showIgnored)
            .FirstOrDefault(c => c.ISOCode.Equals(isoCode)));
    }
    public async Task<bool> IsThereACorrespondingCountryToTheISO2CodeAsync(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true)
    {
        var result = await CreateQuery(showDeleted: showDeleted, showIgnored: showIgnored).AnyAsync(c => c.
            ISOCode.Equals(iso2Code.ToUpperInvariant()));
        return result;
    }

    public bool IsThereACorrespondingCountryToTheISO2Code(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true)
    {
        return RepoDbSet.Any(c => c.ISOCode.Equals(iso2Code.ToUpperInvariant()));
    }

    public async Task<Guid?> GetCountryIdByISOCodeAsync(string iso2Code, string? userId = null, string? roleName = null)
    {
        var result = await RepoDbSet.FirstOrDefaultAsync(c => c.ISOCode.Equals(iso2Code.ToUpperInvariant()));
        return result?.Id;
    }

    public Guid? GetCountryIdByISOCode(string iso2Code, string? userId = null, string? roleName = null)
    {
        var result =  RepoDbSet.FirstOrDefault(c => c.ISOCode.Equals(iso2Code.ToUpperInvariant()));
        return result?.Id;
    }
}