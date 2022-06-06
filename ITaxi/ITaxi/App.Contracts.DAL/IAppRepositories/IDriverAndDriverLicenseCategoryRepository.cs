using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverAndDriverLicenseCategoryRepository: IEntityRepository<DriverAndDriverLicenseCategory>
{
    Task<IEnumerable<string>> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id);
    string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator);

    Task<List<DriverAndDriverLicenseCategory?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id);


}