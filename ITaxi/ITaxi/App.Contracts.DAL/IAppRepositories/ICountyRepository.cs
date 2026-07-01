using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository : IEntityRepository<CountyDTO>,ICountyRepositoryCustom<CountyDTO>
{
    
}
public interface ICountyRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true, bool noIncludes = false, 
        bool showIgnored = false);
    IEnumerable<TEntity> GetAllCountiesOrderedByCountyName(bool noTracking = true, bool noIncludes = false);
    Task<bool> DoesCountyExistsByCountryIdAndEHAKCodeAsync(Guid countryId, string ehakCode);
    bool DoesCountyExistsByCountryIdAndEHAKCode(Guid countryId, string ehakCode);
    Task<TEntity?> GetCountyByEHAKCodeAsync(string ehakCode);
    TEntity?GetCountyByEHAKCode(string ehakCode);
    Task<bool> HasCities(Guid countyId);
    Task<List<TEntity?>>GetAllCountiesOrderedByCountyNameByCountryIdAsync(Guid countryId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false);
    
}