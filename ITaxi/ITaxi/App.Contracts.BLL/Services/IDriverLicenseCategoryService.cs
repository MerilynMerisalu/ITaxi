using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IDriverLicenseCategoryService: IEntityService<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO>,
    IDriverLicenseCategoryRepositoryCustom<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO> // Add custom stuff
{
    
}