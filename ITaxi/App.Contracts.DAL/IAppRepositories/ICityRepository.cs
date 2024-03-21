

using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICityRepository : IEntityRepository<CityDTO>, ICityRepositoryCustom<CityDTO>
{
    
}

public interface ICityRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCitiesWithoutCountyAsync();
    Task<IEnumerable<TEntity>> GetAllOrderedCitiesWithoutCountyAsync();
    Task<IEnumerable<TEntity>> GetAllOrderedCitiesAsync();
    Task<TEntity?> FirstOrDefaultCityWithoutCountyAsync(Guid id);
    IEnumerable<TEntity> GetAllOrderedCitiesWithoutCounty();
    IEnumerable<TEntity> GetAllOrderedCities(bool noTracking = true);
    Task<bool> HasAnyCitiesAsync(Guid id, bool noTracking = true);
    bool HasAnyCities(Guid id, bool noTracking = true);
}