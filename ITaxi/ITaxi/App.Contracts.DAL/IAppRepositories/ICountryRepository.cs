using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;
using System.Threading.Tasks;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountryRepository: IEntityRepository<CountryDTO>, ICountryRepositoryCustom<CountryDTO>
{
    
}

public interface ICountryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true, bool noIncludes = false);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryName(bool noTracking = true, bool noIncludes = false);
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true);
    bool HasAnyCounties(Guid id, bool noTracking = true);
    Task<TEntity?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);
    TEntity? GetCountryByISOCode(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);



}