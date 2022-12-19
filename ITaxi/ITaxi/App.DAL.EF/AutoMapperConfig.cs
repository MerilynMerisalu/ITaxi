using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap(typeof(App.DTO.AdminArea.CountyDTO), typeof(App.Domain.County)).ReverseMap();
        CreateMap(typeof(App.DTO.AdminArea.CityDTO), typeof(App.Domain.City)).ReverseMap();
        CreateMap(typeof(App.DTO.Identity.AppUser), typeof(App.Domain.Identity.AppUser)).ReverseMap();
    }
}