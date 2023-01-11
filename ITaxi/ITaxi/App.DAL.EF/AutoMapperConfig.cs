using App.DAL.DTO.AdminArea;
using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    // DateTime to UTC Converter
    public class ToUtcFormatter : IValueConverter<DateTime, DateTime>
    {
        public DateTime Convert(DateTime localSource, ResolutionContext context) => localSource.ToUniversalTime();
    }
    public class ToLocalFormatter : IValueConverter<DateTime, DateTime>
    {
        public DateTime Convert(DateTime dbSource, ResolutionContext context) => dbSource.ToLocalTime();
    }
    public AutoMapperConfig()
    {
        #region County Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.County, CountyDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            .ForMember(dto => dto.NumberOfCities, m => m.MapFrom(x => x.Cities.Count()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<CountyDTO, App.Domain.County>()
            .ForMember(db => db.CreatedAt,
                dto =>
                    dto.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(db => db.UpdatedAt,
                dto =>
                    dto.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            ;
        #endregion County Mapping
        
        #region City Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.City, CityDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<CityDTO, App.Domain.City>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion City Mapping
        
        #region Admin Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Admin, AdminDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<AdminDTO, App.Domain.Admin>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion Admin Mapping
        
        #region DriverLicenseCategory Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.DriverLicenseCategory, DriverLicenseCategoryDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<DriverLicenseCategoryDTO, App.Domain.DriverLicenseCategory>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion DriverLicenseCategory Mapping
        
        #region Driver Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Driver, DriverDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<DriverDTO, App.Domain.Driver>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion Driver Mapping
        
        #region DriverAndLicenseCategory Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.VehicleType, VehicleTypeDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<VehicleTypeDTO, App.Domain.VehicleType>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion DriverAndLicenseCategory Mapping
        
        CreateMap<DriverAndDriverLicenseCategoryDTO, App.Domain.DriverAndDriverLicenseCategory>()
            .ReverseMap();
        
        #region VehicleMark Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.VehicleMark, VehicleMarkDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<VehicleMarkDTO, App.Domain.VehicleMark>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion VehicleMark Mapping
        
        #region VehicleModel Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.VehicleModel, VehicleModelDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<VehicleModelDTO, App.Domain.VehicleModel>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion VehicleModel Mapping
        
        #region Vehicle Mapping
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Vehicle, VehicleDTO>()
            .ForMember(dto => dto.CreatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            .ForMember(dto => dto.UpdatedAt, m => m.ConvertUsing(new ToLocalFormatter()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<VehicleDTO, App.Domain.Vehicle>()
            .ForMember(db => db.CreatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            .ForMember(db => db.UpdatedAt, dto => dto.ConvertUsing(new ToUtcFormatter()))
            ;       
       
        #endregion Vehicle Mapping
        
        
        CreateMap<AppUser, App.Domain.Identity.AppUser>().ReverseMap();
    }
}