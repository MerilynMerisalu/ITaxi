using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers.Identity;

public class AppUserMapper: BaseMapper<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}
