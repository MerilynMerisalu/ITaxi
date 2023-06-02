using AutoMapper;

namespace App.BLL;
public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.BLL.DTO.AdminArea.CountyDTO, App.DAL.DTO.AdminArea.CountyDTO>()
            .ReverseMap();

        CreateMap<App.BLL.DTO.AdminArea.CityDTO, App.DAL.DTO.AdminArea.CityDTO>()
            .ReverseMap();

        CreateMap<App.BLL.DTO.AdminArea.AdminDTO, App.DAL.DTO.AdminArea.AdminDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO, App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO>()
            .ReverseMap();      
        
        CreateMap<App.BLL.DTO.AdminArea.DriverDTO, App.DAL.DTO.AdminArea.DriverDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO,
                App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleTypeDTO, App.DAL.DTO.AdminArea.VehicleTypeDTO>()
            .ReverseMap();  
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleMarkDTO, App.DAL.DTO.AdminArea.VehicleMarkDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleModelDTO, App.DAL.DTO.AdminArea.VehicleModelDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleDTO, App.DAL.DTO.AdminArea.VehicleDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.ScheduleDTO, App.DAL.DTO.AdminArea.ScheduleDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.RideTimeDTO, App.DAL.DTO.AdminArea.RideTimeDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DisabilityTypeDTO, App.DAL.DTO.AdminArea.DisabilityTypeDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.CustomerDTO, App.DAL.DTO.AdminArea.CustomerDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.BookingDTO, App.DAL.DTO.AdminArea.BookingDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DriveDTO, App.DAL.DTO.AdminArea.DriveDTO>()
            .ReverseMap()
            .ForMember(x => x.DriveDescription, m => 
                    m.MapFrom(dto => $"{dto.Booking!.PickUpDateAndTime:g} - {dto.Driver!.AppUser!.LastAndFirstName}"));
        
        CreateMap<App.BLL.DTO.AdminArea.CommentDTO, App.DAL.DTO.AdminArea.CommentDTO>()
            .ReverseMap()
            .ForMember(x => 
                x.DriveCustomerStr, m =>
                m.MapFrom(dto => $"{dto!.Drive.Booking!.PickUpDateAndTime:g}"))
            .ForMember(x => x.DriverName, m => m.MapFrom(dto => dto.Drive.Driver.AppUser.LastAndFirstName))
            .ForMember(x => x.CustomerName, m => 
                m.MapFrom(dto => dto.Drive.Booking.Customer.AppUser.LastAndFirstName))
            .ForMember(dto => dto.DriveTimeAndDriver, m => 
                m.MapFrom(dto => $"{dto.Drive.Booking.PickUpDateAndTime:g} - {dto.Drive.Driver.AppUser.LastAndFirstName}"));
        
        CreateMap<App.BLL.DTO.AdminArea.PhotoDTO, App.DAL.DTO.AdminArea.PhotoDTO>()
            .ReverseMap();
    }
}