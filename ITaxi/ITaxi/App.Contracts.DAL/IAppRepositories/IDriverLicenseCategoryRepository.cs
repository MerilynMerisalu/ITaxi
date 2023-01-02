
using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverLicenseCategoryRepository : IEntityRepository<DriverLicenseCategoryDTO>, IDriverLicenseCategoryRepositoryCustom<DriverLicenseCategoryDTO>
{
    
}

public interface IDriverLicenseCategoryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllDriverLicenseCategoriesOrderedAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true);
}
