
using AutoMapper;

namespace WebApp.ApiControllers.v1;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.Public.DTO.v1.AdminArea.County, App.BLL.DTO.AdminArea.CountyDTO>()
            .ReverseMap()
            ;

        CreateMap<App.Public.DTO.v1.AdminArea.City, App.BLL.DTO.AdminArea.CityDTO>()
            .ReverseMap()
           ;
        

        CreateMap<App.Public.DTO.v1.AdminArea.Admin, App.BLL.DTO.AdminArea.AdminDTO>()
            .ReverseMap()
           ;
        /*
        
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO, App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO>()
            .ReverseMap()
           ;        
        CreateMap<App.BLL.DTO.AdminArea.DriverDTO, App.DAL.DTO.AdminArea.DriverDTO>()
            .ReverseMap()
           ;
        
        CreateMap<App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO,
                App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>()
            .ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleTypeDTO, App.DAL.DTO.AdminArea.VehicleTypeDTO>()
            .ReverseMap()
;        
        CreateMap<App.BLL.DTO.AdminArea.VehicleMarkDTO, App.DAL.DTO.AdminArea.VehicleMarkDTO>()
            .ReverseMap()
           ;
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleModelDTO, App.DAL.DTO.AdminArea.VehicleModelDTO>()
            .ReverseMap()
           ;
        
        CreateMap<App.BLL.DTO.AdminArea.VehicleDTO, App.DAL.DTO.AdminArea.VehicleDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.ScheduleDTO, App.DAL.DTO.AdminArea.ScheduleDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.RideTimeDTO, App.DAL.DTO.AdminArea.RideTimeDTO>()
            .ReverseMap();
        CreateMap<App.BLL.DTO.AdminArea.DisabilityTypeDTO, App.DAL.DTO.AdminArea.DisabilityTypeDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.CustomerDTO, App.DAL.DTO.AdminArea.CustomerDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.BookingDTO, App.DAL.DTO.AdminArea.BookingDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.DriveDTO, App.DAL.DTO.AdminArea.DriveDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.CommentDTO, App.DAL.DTO.AdminArea.CommentDTO>()
            .ReverseMap()
            ;
        CreateMap<App.BLL.DTO.AdminArea.PhotoDTO, App.DAL.DTO.AdminArea.PhotoDTO>()
            .ReverseMap()
            ;*/
    }
    
    
}