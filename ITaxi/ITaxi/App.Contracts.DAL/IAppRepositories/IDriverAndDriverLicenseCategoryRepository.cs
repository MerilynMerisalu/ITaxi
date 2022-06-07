using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverAndDriverLicenseCategoryRepository: IEntityRepository<DriverAndDriverLicenseCategory>
{
    Task<string?> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id, string separator = ", ");
    string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = ", ");

    Task<List<DriverAndDriverLicenseCategory?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id);
    


}