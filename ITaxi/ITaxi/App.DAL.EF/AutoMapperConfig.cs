using App.DAL.DTO.AdminArea;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<CountyDTO, App.Domain.County>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt, 
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt, 
                m => 
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            .ForMember(dto => dto.NumberOfCities, m => m.MapFrom(x => x.Cities.Count()));
        CreateMap<CityDTO, App.Domain.City>()
            .ReverseMap()
        .ForMember(dto => dto.CreatedAt, 
                m => 
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
        .ForMember(dto => dto.UpdatedAt, 
                m => 
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<AppUser, App.Domain.Identity.AppUser>().ReverseMap();
    }
}