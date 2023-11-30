using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICustomerService: IEntityService<App.BLL.DTO.AdminArea.CustomerDTO>, 
    ICustomerRepositoryCustom<App.BLL.DTO.AdminArea.CustomerDTO>
{
    
}