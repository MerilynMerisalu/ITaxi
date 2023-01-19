using App.DAL.DTO.Identity;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IAppUserRepository : IEntityRepository<AppUser>, IAppUserRepositoryCustom<AppUser>
{
}
public interface IAppUserRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAppUsersOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllAppUsersOrderedByLastName(bool noTracking = true);
}
