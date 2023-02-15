

using App.DAL.DTO.AdminArea;
using App.DAL.DTO.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IAdminRepository : IEntityRepository<AdminDTO>, IAdminRepositoryCustom<AdminDTO>
{
   
}
public interface IAdminRepositoryCustom<TEntity> 
{
    Task<IEnumerable<TEntity>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllAdminsOrderedByLastName(bool noTracking = true);
   
}