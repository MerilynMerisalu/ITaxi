using App.DAL.DTO.AdminArea;
using App.DAL.DTO.Identity;
using App.Domain;
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
            .ForMember(dto => dto.NumberOfCities, m
                => m.MapFrom(x => x.Cities.Count()))
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
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.DisabilityType, DisabilityTypeDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            ;
        // DTO => EF: Convert to Universal Time
        CreateMap<DisabilityTypeDTO, App.Domain.DisabilityType>()
            .ForMember(db => db.CreatedAt,
                dto =>
                    dto.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(db => db.UpdatedAt,
                dto =>
                    dto.MapFrom(x => x.UpdatedAt.ToUniversalTime()));

        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Schedule, ScheduleDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            .ForMember(dto => dto.StartDateAndTime,
                m =>
                    m.MapFrom(x => x.StartDateAndTime.ToLocalTime()))
            .ForMember(dto => dto.EndDateAndTime,
                m =>
                    m.MapFrom(x => x.EndDateAndTime.ToLocalTime()))
            .ForMember(dto => dto.NumberOfRideTimes, 
                m
                => m.MapFrom(x => x.RideTimes!.Count))
            .ForMember(dto => dto.NumberOfTakenRideTimes, 
                m
                => m.MapFrom(x => x.RideTimes!.Count(x => x.IsTaken)));
            

        // DTO => EF: Convert to Universal Time
        CreateMap<ScheduleDTO, App.Domain.Schedule>()
            .ForMember(db => db.CreatedAt,
                dto =>
                    dto.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(db => db.UpdatedAt,
                dto =>
                    dto.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            .ForMember(dto => dto.StartDateAndTime,
                dto => dto
                    .MapFrom(x => x.StartDateAndTime.ToUniversalTime()))
            .ForMember(dto => dto.EndDateAndTime,
                dto => dto
                    .MapFrom(x => x.EndDateAndTime.ToUniversalTime()));
        
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.RideTime, RideTimeDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            
            .ForMember(dto => dto.RideDateTime,
                dto => dto
                    .MapFrom(x => x.RideDateTime.ToLocalTime()));
        
        // DTO => EF: Convert to Universal Time
        CreateMap<RideTimeDTO, App.Domain.RideTime>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            .ForMember(dto => dto.RideDateTime,
                dto => dto
                    .MapFrom(x => x.RideDateTime.ToUniversalTime()));
        
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Customer, CustomerDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()));
            
        // DTO => EF: Convert to Universal Time
        CreateMap<CustomerDTO, App.Domain.Customer>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToUniversalTime()));
        
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Booking, BookingDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            .ForMember(dto => dto.PickUpDateAndTime,
                m =>
                    m.MapFrom(x => x.PickUpDateAndTime.ToLocalTime()));
            
        // DTO => EF: Convert to Universal Time
        CreateMap<BookingDTO, App.Domain.Booking>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            .ForMember(dto => dto.PickUpDateAndTime,
                m =>
                    m.MapFrom(x => x.PickUpDateAndTime.ToUniversalTime()));

        CreateMap<App.Domain.Drive, DriveDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            .ForMember(dto => dto.DriveAcceptedDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveAcceptedDateAndTime.ToLocalTime()))
            .ForMember(dto => dto.DriveDeclineDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveDeclineDateAndTime.ToLocalTime()))
            .ForMember(dto => dto.DriveStartDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveStartDateAndTime.ToLocalTime()))
            .ForMember(dto => dto.DriveEndDateAndTime,
                x =>
                    x.MapFrom(m => m.DriveEndDateAndTime.ToLocalTime()));

        // DTO => EF: Convert to Universal Time
        CreateMap<DriveDTO, App.Domain.Drive>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            .ForMember(dto => dto.DriveAcceptedDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveAcceptedDateAndTime.ToUniversalTime()))
            .ForMember(dto => dto.DriveDeclineDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveDeclineDateAndTime.ToUniversalTime()))
            .ForMember(dto => dto.DriveStartDateAndTime,
                m =>
                    m.MapFrom(x => x.DriveStartDateAndTime.ToUniversalTime()))
            .ForMember(dto => dto.DriveEndDateAndTime,
                x =>
                    x.MapFrom(m => m.DriveEndDateAndTime.ToUniversalTime()));
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Comment, CommentDTO>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<CommentDTO, App.Domain.Comment>()
            .ForMember(db => db.CreatedAt,
                dto =>
                    dto.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(db => db.UpdatedAt,
                dto =>
                    dto.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            ;
        // Convert from EF => DTO: Convert to Local Time
        CreateMap<App.Domain.Photo, Photo>()
            .ForMember(dto => dto.CreatedAt,
                m =>
                    m.MapFrom(x => x.CreatedAt.ToLocalTime()))
            .ForMember(dto => dto.UpdatedAt,
                m =>
                    m.MapFrom(x => x.UpdatedAt.ToLocalTime()))
            ;

        // DTO => EF: Convert to Universal Time
        CreateMap<PhotoDTO, App.Domain.Photo>()
            .ForMember(db => db.CreatedAt,
                dto =>
                    dto.MapFrom(x => x.CreatedAt.ToUniversalTime()))
            .ForMember(db => db.UpdatedAt,
                dto =>
                    dto.MapFrom(x => x.UpdatedAt.ToUniversalTime()))
            ;


        CreateMap<AppUser, App.Domain.Identity.AppUser>().ReverseMap();
    }
}