using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository : IEntityRepository<CountyDTO>,ICountyRepositoryCustom<CountyDTO>
{
    
}
public interface ICountyRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true, bool noIncludes = false);
    IEnumerable<TEntity> GetAllCountiesOrderedByCountyName(bool noTracking = true, bool noIncludes = false);
    Task<bool> IsThereACorrespondingCountryToTheISO2CodeAsync(string iso2Code, string? userId = null, string? roleName = null, bool noIncludes = false);
    bool IsThereACorrespondingCountryToTheISO2Code(string iso2Code, string? userId = null, string? roleName = null, bool noIncludes = false);
    Task<bool> HasCities(Guid countyId);
}