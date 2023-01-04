using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverAndDriverLicenseCategoryRepository : IEntityRepository<DriverAndDriverLicenseCategoryDTO>
{
    Task<string?> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id, string separator = ", ");
    string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = ", ");

    Task<List<DriverAndDriverLicenseCategoryDTO?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id);
    
    
    
}