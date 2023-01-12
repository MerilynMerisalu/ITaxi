using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverAndDriverLicenseCategoryRepository : 
    IEntityRepository<DriverAndDriverLicenseCategoryDTO>, 
    IDriverAndDriverLicenseCategoryRepositoryCustom<DriverAndDriverLicenseCategoryDTO>
{
    
    
}
public interface IDriverAndDriverLicenseCategoryRepositoryCustom<TEntity>
{
    Task<string?> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id, string separator = ", ");
    string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = ", ");

    Task<List<TEntity?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id);
    Task<bool> HasAnyDriversAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);


}