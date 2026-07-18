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
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true);
    Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true);
    bool HasAnyCounties(Guid id, bool noTracking = true);
    Task<TEntity?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);
    TEntity? GetCountryByISOCode(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true);
    Task<bool> IsThereACorrespondingCountryToTheISO2CodeAsync(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true);
    bool IsThereACorrespondingCountryToTheISO2Code(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true);
    Task<Guid?> GetCountryIdByISOCodeAsync(string iso2Code, string? userId = null, string? roleName = null);
    Guid? GetCountryIdByISOCode(string iso2Code, string? userId = null, string? roleName = null);
    Task<IEnumerable<TEntity?>> GetAllCountriesWhereIsRegisterSupportedAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false);
    IEnumerable<TEntity?> GetAllCountriesWhereIsRegisterSupported(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false);


}