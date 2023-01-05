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
        CreateMap<AdminDTO, App.Domain.Admin>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<DriverLicenseCategoryDTO, App.Domain.DriverLicenseCategory>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
        CreateMap<DriverDTO, App.Domain.Driver>()
            .ReverseMap()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));

        CreateMap<DriverAndDriverLicenseCategoryDTO, App.Domain.DriverAndDriverLicenseCategory>()
            .ReverseMap();
        CreateMap<VehicleTypeDTO, App.Domain.VehicleType>()
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