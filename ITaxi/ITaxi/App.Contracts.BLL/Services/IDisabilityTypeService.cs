using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IDisabilityTypeService: IEntityService<App.BLL.DTO.AdminArea.DisabilityTypeDTO>,
    IDisabilityTypeRepositoryCustom<App.BLL.DTO.AdminArea.DisabilityTypeDTO>
{
    
}