﻿using App.Contracts.DAL.IAppRepositories;
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
        bool noTracking = true, bool noIncludes = false)
    {
        return (await CreateQuery(noTracking, noIncludes).OrderBy(c => c.CountyName).ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true, bool noIncludes = false)
    {
        return CreateQuery(noTracking, noIncludes).OrderBy(c => c.CountyName)
            .Select(e => Mapper.Map(e)!);
    }

    

    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountryName(bool noTracking = true, bool noIncludes = false)
    {
        // special handling of OrderBy to account for language transalation
        return CreateQuery(noTracking)
            .ToList() // Bring into memory "Materialize"
            .OrderBy(v => v.Country!.CountryName)
            
            .ToList().Select(e => Mapper.Map(e))!;
    }
    

    public async Task<bool> HasCities(Guid countyId)
    {
        return await RepoDbContext.Cities.AnyAsync(x => x.CountyId == countyId);
    }
    
    protected override IQueryable<App.Domain.County> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted);
        if (!noIncludes)
            query = query.Include(x => x.Cities)
                .Include(x => x.Country)
                .ThenInclude(x => x.CountryName)
                .ThenInclude(x => x.Translations);
            
        if (noTracking) query = query.AsNoTracking();

        return query;
    }
}