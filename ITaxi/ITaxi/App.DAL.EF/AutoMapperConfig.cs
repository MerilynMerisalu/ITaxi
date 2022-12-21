using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.DTO.AdminArea.CountyDTO, App.Domain.County>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, m => m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, m => m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<App.DTO.AdminArea.CityDTO, App.Domain.City>()
            .ReverseMap();
            //.ForMember(dto => dto.CreatedAt, m => m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            //.ForMember(dto => dto.UpdatedAt, m => m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<App.DTO.Identity.AppUser, App.Domain.Identity.AppUser>().ReverseMap();
    }
}