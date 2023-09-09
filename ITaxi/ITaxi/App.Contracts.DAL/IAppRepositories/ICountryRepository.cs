using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountryRepository: IEntityRepository<CountryDTO>, ICountryRepositoryCustom<CountryDTO>
{
    
}

public interface ICountryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryName(bool noTracking = true);
    
}