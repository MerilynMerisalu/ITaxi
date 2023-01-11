
using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.BLL.DTO.AdminArea.CountyDTO, App.DAL.DTO.AdminArea.CountyDTO>()
            .ReverseMap()
            ;

        CreateMap<App.BLL.DTO.AdminArea.CityDTO, App.DAL.DTO.AdminArea.CityDTO>()
            .ReverseMap()
           ;

        CreateMap<App.BLL.DTO.AdminArea.AdminDTO, App.DAL.DTO.AdminArea.AdminDTO>()
            .ReverseMap()
           ;
        
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
    }
    
}