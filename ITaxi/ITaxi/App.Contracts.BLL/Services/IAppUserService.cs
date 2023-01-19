using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAppUserService : IEntityService<App.BLL.DTO.Identity.AppUser>,
    IAppUserRepositoryCustom<App.BLL.DTO.Identity.AppUser>
{

}