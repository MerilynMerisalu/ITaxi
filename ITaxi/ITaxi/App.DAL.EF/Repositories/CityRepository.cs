﻿using System.Linq.Expressions;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CityRepository : BaseEntityRepository<CityDTO, City, AppDbContext>, ICityRepository
{
    public CityRepository(AppDbContext dbContext, IMapper<CityDTO, App.Domain.City> mapper) :
        base(dbContext, mapper)
    {
    }

    public virtual async Task<IEnumerable<CityDTO>> GetAllCitiesWithoutCountyAsync()
    {
        var res = (await base.CreateQuery().ToListAsync()).Select(e => Mapper.Map(e));
        return res!;
    }

    public async Task<IEnumerable<CityDTO>> GetAllOrderedCitiesWithoutCountyAsync()
    {
        var res = (await base.CreateQuery().OrderBy(c => c.CityName).ToListAsync()).
            Select(e => Mapper.Map(e));
        return res!;
    }

    public async Task<IEnumerable<CityDTO>> GetAllOrderedCitiesAsync()
    {
        return (await CreateQuery().OrderBy(c => c.County!.CountyName)
            .ThenBy(c => c.CityName).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<CityDTO?> FirstOrDefaultCityWithoutCountyAsync(Guid id)
    {
        var res = await base.CreateQuery().FirstOrDefaultAsync(c => c.Id.Equals(id));
        return Mapper.Map(res);
    }

    public IEnumerable<CityDTO> GetAllOrderedCitiesWithoutCounty()
    {
        return base.CreateQuery().OrderBy(c => c.CityName).ToList()
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CityDTO> GetAllOrderedCities(bool noTracking =true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasAnyCitiesAsync(Guid id, bool noTracking = true)
    {
        #warning violating of the DRY pattern
        return await CreateQuery(noTracking).AnyAsync(c => c.CountyId.Equals(id));
    }

    public bool HasAnyCities(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).Any(c => c.CountyId.Equals(id));
    }

    public async Task<IEnumerable<CityDTO>> GettingByCountyIdAsync(Guid countyId)
    {
        return (await CreateQuery().Where(x => x.CountyId == countyId).ToListAsync()).Select(x => Mapper.Map(x))!;
    }

    public override async Task<CityDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        query = query.Include(c => c.County);

        var res = await query.FirstOrDefaultAsync(c => c.Id == id);

        return Mapper.Map(res);
    }


    public override CityDTO? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id)));
    }

    public async /*override */ Task<CityDTO?> SingleOrDefaultAsync(Expression<Func<City?, bool>> filter, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).SingleOrDefaultAsync(c => c.Id.Equals(filter)));
    }

    public /*override */ CityDTO? SingleOrDefault(Expression<Func<City?, bool>> filter, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).SingleOrDefault(e => e.Id.Equals(filter)));
    }

    protected override IQueryable<City> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        query = query.Include(c => c.County);
        return query;
    }
}