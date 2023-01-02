
using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriverLicenseCategoryRepository : IEntityRepository<DriverLicenseCategoryDTO>
{
    Task<IEnumerable<DriverLicenseCategoryDTO>> GetAllDriverLicenseCategoriesOrderedAsync(bool noTracking = true);
    IEnumerable<DriverLicenseCategoryDTO> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true);
}