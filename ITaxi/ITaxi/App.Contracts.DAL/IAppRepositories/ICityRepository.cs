using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICityRepository: IEntityRepository<City>
{
    Task<IEnumerable<City>> GetAllCitiesWithoutCountyAsync();
    Task<City?> FirstOrDefaultCityWithoutCountyAsync(Guid id);
}