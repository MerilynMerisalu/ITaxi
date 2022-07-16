using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverRepository : IEntityRepository<Driver>
{
    Task<IEnumerable<Driver>>GetAllDriversOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<Driver> GetAllDriversOrderedByLastName(bool noTracking = true);
}