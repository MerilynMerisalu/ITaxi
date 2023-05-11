
using AutoMapper;
using Base.Domain;

namespace WebApp.ApiControllers.v1;

public class LangStrTypeConverter : ITypeConverter<LangStr, string>
{
    private IHttpContextAccessor _httpContext;
    public LangStrTypeConverter(IHttpContextAccessor context)
    {
        _httpContext = context;
    }
    /// <summary>
    /// Convert a LangStr to string using the current Http Request Language
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Convert(LangStr source, string destination, ResolutionContext context)
    {
        string? lang = _httpContext?.HttpContext?.Request?.Headers?.AcceptLanguage.FirstOrDefault();
        return source.Translate(lang)!; // the underlying string can be null, even if there are translations!
    }
}
public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        // Globally convert all LangStr using Request Headers, not the current Thread!
        CreateMap<LangStr, string>().ConvertUsing<LangStrTypeConverter>();
        
        
        CreateMap<App.Public.DTO.v1.AdminArea.County, App.BLL.DTO.AdminArea.CountyDTO>()
            .ReverseMap()
            ;

        CreateMap<App.Public.DTO.v1.AdminArea.City, App.BLL.DTO.AdminArea.CityDTO>()
            .ReverseMap()
           ;
        

        CreateMap<App.Public.DTO.v1.AdminArea.Admin, App.BLL.DTO.AdminArea.AdminDTO>()
            .ReverseMap()
           ;
        
        
        CreateMap<App.Public.DTO.v1.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.DriverLicenseCategory,
                App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO>()
            .ReverseMap();
        

        CreateMap<App.Public.DTO.v1.Identity.AdminRegistration,
                WebApp.DTO.AdminRegistrationDTO>()
            .ReverseMap();
        CreateMap<App.Public.DTO.v1.AdminArea.Driver, App.BLL.DTO.AdminArea.DriverDTO>()
            .ReverseMap();
        CreateMap<App.Public.DTO.v1.AdminArea.DisabilityType, App.BLL.DTO.AdminArea.DisabilityTypeDTO>()
            .ReverseMap()
           ;        
        /*
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