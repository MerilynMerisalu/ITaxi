using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverLicenseCategoryRepository: IEntityRepository<DriverLicenseCategory>
{
    Task<IEnumerable<DriverLicenseCategory>> GetAllDriverLicenseCategoriesOrderedAsync(bool noTracking = true);
    IEnumerable<DriverLicenseCategory> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true);
}