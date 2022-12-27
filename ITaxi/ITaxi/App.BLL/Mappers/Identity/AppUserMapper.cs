using App.DAL.DTO.Identity;
using Base.Contracts;

namespace App.BLL.Mappers.Identity;

public class AppUserMapper: IMapper<AppUser, App.Domain.Identity.AppUser>
{
    public AppUser? Map(Domain.Identity.AppUser? entity)
    {
        throw new NotImplementedException();
    }

    public Domain.Identity.AppUser? Map(AppUser? entity)
    {
        throw new NotImplementedException();
    }
}