using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IDriverAndDriverLicenseCategoryService: 
    IEntityService<App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>
    ,IDriverAndDriverLicenseCategoryRepositoryCustom<App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>
    // Add custom stuff
{
    
}