using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICityRepository : IEntityRepository<City>
{
    Task<IEnumerable<City>> GetAllCitiesWithoutCountyAsync();
    Task<IEnumerable<City>> GetAllOrderedCitiesWithoutCountyAsync();
    Task<IEnumerable<City>> GetAllOrderedCitiesAsync();
    Task<City?> FirstOrDefaultCityWithoutCountyAsync(Guid id);
    IEnumerable<City> GetAllOrderedCitiesWithoutCounty();
}