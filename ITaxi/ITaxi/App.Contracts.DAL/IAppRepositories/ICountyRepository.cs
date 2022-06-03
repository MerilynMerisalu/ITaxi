using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository: IEntityRepository<County>
{
    Task<IEnumerable<County>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true);
   IEnumerable<County> GetAllCountiesOrderedByCountyName(bool noTracking = true);
}