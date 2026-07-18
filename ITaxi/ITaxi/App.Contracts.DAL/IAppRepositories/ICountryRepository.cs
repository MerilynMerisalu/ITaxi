using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;
using System.Threading.Tasks;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountryRepository: IEntityRepository<CountryDTO>, ICountryRepositoryCustom<CountryDTO>
{
    
}

public interface ICountryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true, bool noIncludes = false, bool showIgnored = false);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryName(bool noTracking = true, bool noIncludes = false, bool showIgnored = false, bool showDeleted = false);
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryISOCca2CodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryISOCca2Code(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true);
    bool HasAnyCounties(Guid id, bool noTracking = true);
    Task<TEntity?> GetCountryByISOCodeCca2Async(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);
    TEntity? GetCountryByISOCodeCca2(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);
    Task<bool> IsThereACorrespondingCountryToTheISO2CodeAsync(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true);
    bool IsThereACorrespondingCountryToTheISO2Code(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true);
    Task<Guid?> GetCountryIdByISOCodeCca2Async(string iso2Code, string? userId = null, string? roleName = null);
    Guid? GetCountryIdByISOCca2Code(string iso2Code, string? userId = null, string? roleName = null);
    Task<IEnumerable<TEntity?>> GetAllCountriesWhereIsRegisterSupportedAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false);
    IEnumerable<TEntity?> GetAllCountriesWhereIsRegisterSupported(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false);


}