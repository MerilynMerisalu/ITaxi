using App.BLL.DTO.AdminArea;
using App.DAL.DTO.Identity;
using AutoMapper;
using Base.Contracts;
using Base.DAL;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace App.BLL.Mappers.Identity;

public class AppUserMapper: BaseMapper<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}
