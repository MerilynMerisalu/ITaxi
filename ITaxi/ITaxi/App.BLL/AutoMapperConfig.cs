using App.DAL.DTO.AdminArea;
using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.BLL.DTO.AdminArea.CountyDTO, App.DAL.DTO.AdminArea.CountyDTO>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, m => m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, m => 
                m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<App.BLL.DTO.AdminArea.CityDTO, App.DAL.DTO.AdminArea.CityDTO>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, m
                => m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, m
                => m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<App.BLL.DTO.AdminArea.AdminDTO, App.DAL.DTO.AdminArea.AdminDTO>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, 
                m => 
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, 
                m => 
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO, App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, 
                m => 
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, 
                m => 
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
    }
}