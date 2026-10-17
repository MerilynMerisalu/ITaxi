

using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IMunicipalityRepository : IEntityRepository<MunicipalityDTO>, IMunicipalityCustom<MunicipalityDTO>
{
    
}

public interface IMunicipalityCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllMunicipalitiesWithoutCountyAsync();
    Task<IEnumerable<TEntity>> GetAllOrderedMunicipalitiesWithoutCountyAsync();
    Task<IEnumerable<TEntity>> GetAllOrderedMunipalitiesAsync(bool showDeleted = false, bool showIgnored = false);
    Task<TEntity?> FirstOrDefaultMunicipalityWithoutCountyAsync(Guid id, bool showIgnored = false);
    IEnumerable<TEntity> GetAllOrderedMunicipalitiesWithoutCounty();
    IEnumerable<TEntity> GetAllOrderedMunicipalities(bool noTracking = true);
    Task<bool> HasAnyMunicipalitiesAsync(Guid id, bool noTracking = true);
    bool HasAnyMunicipalities(Guid id, bool noTracking = true);
    Task<TEntity?>GetMunicipalityByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false);
    Task<List<TEntity?>> GetMunicipalitiesByCountyIdAsync(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false);
    List<TEntity?> GetMunicipalitiesByCountyId(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false);
    Task<bool> DoesMunicipalityExistAsync(string ehakCode);
}