
using App.BLL.DTO.Identity;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class AppUserService : BaseEntityService<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser, IAppUserRepository>,
    IAppUserService
{
    public AppUserService(IAppUserRepository repository, IMapper<AppUser, DAL.DTO.Identity.AppUser> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<AppUser>> GetAllAppUsersOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAppUsersOrderedByLastNameAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<AppUser> GetAllAppUsersOrderedByLastName(bool noTracking = true)
    {
        return Repository.GetAllAppUsersOrderedByLastName(noTracking).Select(e => Mapper.Map(e))!;
    }

}