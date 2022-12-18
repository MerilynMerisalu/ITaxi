using App.DTO.Identity;
using Base.Contracts;

namespace App.DAL.EF.Mappers.Identity;

public class AppUserMapper: IMapper<App.DTO.Identity.AppUser, App.Domain.Identity.AppUser>
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