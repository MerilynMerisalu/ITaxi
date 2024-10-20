﻿using App.Contracts.DAL.IAppRepositories;
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

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true,
        bool noIncludes = false)
    {
        return (await CreateQuery(noTracking, noIncludes).ToListAsync()).Select(c => Mapper.Map(c))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(bool noTracking = true,
    bool noIncludes = false )
    {
        return CreateQuery(noTracking, noIncludes).Select(c => Mapper.Map(c))!;
    }

    public async Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true)
    {
        return await RepoDbContext.Counties.AnyAsync(c => c.CountryId.Equals(id));
    }

    public bool HasAnyCounties(Guid id, bool noTracking = true)
    {
        return RepoDbContext.Counties.Any(c => c.CountryId.Equals(id));
    }

    

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        // special handling of OrderBy to account for language transalation
        return (await CreateQuery(noTracking, showDeleted: showDeleted)
            .ToListAsync()) // Bring into memory "Materialize"
            .OrderBy(v => v.ISOCode)
            .ToList().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false)
    {
        // special handling of OrderBy to account for language transalation
        return CreateQuery(noTracking)
            .ToList() // Bring into memory "Materialize"
            .OrderBy(v => v.ISOCode)
            
            .ToList().Select(e => Mapper.Map(e))!;
    }

    protected override IQueryable<Country> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = true)
    {
        //if (noTracking && showDeleted == true)
        //{
        //   return RepoDbSet
        //        .Include(c => c.CountryName)
        //        .ThenInclude(c => c.Translations!.Where(c => c.IsDeleted == true))
        //       .Where(c => c.IsDeleted == true)
        //        .AsNoTracking();
        //}
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

    public override async Task<CountryDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return (Mapper.Map(await CreateQuery(noTracking, noIncludes)
            .FirstOrDefaultAsync(c => c.Id.Equals(id))));
    }

    public override CountryDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(c => c.Id.Equals(id)));
    }

    public async Task<CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes, showDeleted).FirstOrDefault(c => c.ISOCode.Equals(isoCode)));
    }

    public async Task<CountryDTO?> ToggleCountryIsIgnoredAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        var country = await FirstOrDefaultAsync(id, noTracking, noIncludes);
        if (country == null)
        {
            return null;
        }
        country.IsIgnored = !country.IsIgnored;
        return country;
    }
}