using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository : IEntityRepository<CountyDTO>,ICountyRepositoryCustom<CountyDTO>
{
    
}
public interface ICountyRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllCountiesOrderedByCountyName(bool noTracking = true);

    Task<bool> HasCities(Guid countyId);
}