using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class EhakUnitTypeRepository: BaseEntityRepository<App.DAL.DTO.AdminArea.EhakUnitTypeDTO, 
    App.Domain.EhakUnitType, AppDbContext>, IEhakUnitTypeRepository
{
    public EhakUnitTypeRepository(AppDbContext dbContext, 
        IMapper<EhakUnitTypeDTO, EhakUnitType> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<bool> DoesEhakUnitTypeExistByNameAsync(string name, bool noTracking = true)
    {
        var translations = await RepoDbContext.Translations.ToListAsync();
        var translationId = translations.Where(t => t.Value.Equals(name))
            .Select(t => t.Id);
        
        return translationId != null ? true : false;
    }

    public async Task<Guid?> GetEhakUnitTypeIdAsync(string name, bool noTracking = true)
    {
        var translations = await RepoDbContext.Translations.ToListAsync();
        var translationId = translations.Where(t => t.Value.Equals(name))
            .Select(t => t.Id);
        var ehakUnitTypeId = (await CreateQuery(noTracking: noTracking, noIncludes: false)
            .Where(e => e.Equals(translationId)).FirstOrDefaultAsync()).Id;
        return ehakUnitTypeId;
    }

    protected override IQueryable<EhakUnitType> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted, showIgnored);
        if (!noIncludes)
            query = query.Include(e => e.EhakUnitTypeName)
                         .ThenInclude(t => t.Translations);
        return query;
    }
}