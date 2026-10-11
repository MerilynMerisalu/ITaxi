using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF.Mappers;
using App.Domain;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Contracts.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.DAL.EF.Repositories;

public class MunicipalityRepository : BaseEntityRepository<MunicipalityDTO, App.Domain.Municipality , AppDbContext>, IMunicipalityRepository
{
    public MunicipalityRepository(AppDbContext dbContext, IMapper<MunicipalityDTO, App.Domain.Municipality> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<MunicipalityDTO?> FirstOrDefaultMunicipalityWithoutCountyAsync(Guid id, bool showIgnored = false)
    {
        var res = await base.CreateQuery(noIncludes: true, showIgnored: showIgnored).FirstOrDefaultAsync(c => c.Id.Equals(id));
        return Mapper.Map(res);
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllMunicipalitiesWithoutCountyAsync()
    {
        return base.CreateQuery(noIncludes: true).OrderBy(m => m.Name).ToList()
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalities(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(m => m.Name).ToList().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalitiesWithoutCounty()
    {
        return base.CreateQuery(noIncludes: true).OrderBy(c => c.Name).ToList()
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunicipalitiesWithoutCountyAsync()
    {
        return (await base.CreateQuery(noIncludes: true).OrderBy(c => c.Name).ToListAsync())
           .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunipalitiesAsync(bool showDeleted = false, bool showIgnored = false)
    {
        return (await CreateQuery(showIgnored: showIgnored, showDeleted: showDeleted).OrderBy(c => c.County!.CountyName)
            .ThenBy(c => c.Name).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public List<MunicipalityDTO?> GetMunicipalitiesByCountyId(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var result = CreateQuery(noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)
            .Where(c => c.CountyId == countyId).OrderBy(c => c.Name).ToList();
        return result.Select(c => Mapper.Map(c)).ToList();
    
    }

    public async Task<List<MunicipalityDTO?>> GetMunicipalitiesByCountyIdAsync(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var result = await CreateQuery(noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)
            .Where(c => c.CountyId == countyId).OrderBy(c => c.Name).ToListAsync();
        return result.Select(c => Mapper.Map(c)).ToList();
    }
    

    public async Task<MunicipalityDTO?> GetMunicipalityByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var result = (await CreateQuery(noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored).FirstOrDefaultAsync(c => c.Id == id));
        return Mapper.Map(result);
    }

    public bool HasAnyMunicipalities(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).Any(c => c.CountyId.Equals(id));
    }

    public async Task<bool> HasAnyMunicipalitiesAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).AnyAsync(c => c.CountyId.Equals(id));
    }

    
    
}