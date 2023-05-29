
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
            .ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.City, App.BLL.DTO.AdminArea.CityDTO>()
            .ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.Admin, App.BLL.DTO.AdminArea.AdminDTO>()
            .ReverseMap();

        CreateMap<App.Public.DTO.v1.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.DriverLicenseCategory,
                App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.Identity.AdminRegistration,
                WebApp.DTO.AdminRegistrationDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Driver, App.BLL.DTO.AdminArea.DriverDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.DisabilityType, App.BLL.DTO.AdminArea.DisabilityTypeDTO>().ReverseMap();
       
        CreateMap<App.Public.DTO.v1.AdminArea.Vehicle, App.BLL.DTO.AdminArea.VehicleDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.DriverArea.Vehicle, App.BLL.DTO.AdminArea.VehicleDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Driver, App.BLL.DTO.AdminArea.DriverDTO>()
            .ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.DriverAndDriverLicenseCategory,
                App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.VehicleType, App.BLL.DTO.AdminArea.VehicleTypeDTO>().ReverseMap()
;        
         CreateMap<App.Public.DTO.v1.AdminArea.VehicleMark, App.BLL.DTO.AdminArea.VehicleMarkDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.VehicleModel, App.BLL.DTO.AdminArea.VehicleModelDTO>().ReverseMap();

        CreateMap<App.Public.DTO.v1.AdminArea.Schedule, App.BLL.DTO.AdminArea.ScheduleDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.DriverArea.Schedule, App.BLL.DTO.AdminArea.ScheduleDTO>().ReverseMap();
       
        CreateMap<App.Public.DTO.v1.AdminArea.RideTime, App.BLL.DTO.AdminArea.RideTimeDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.DriverArea.RideTime, App.BLL.DTO.AdminArea.RideTimeDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Customer, App.BLL.DTO.AdminArea.CustomerDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Booking, App.BLL.DTO.AdminArea.BookingDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.CustomerArea.Booking, App.BLL.DTO.AdminArea.BookingDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Drive, App.BLL.DTO.AdminArea.DriveDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.DriverArea.Drive, App.BLL.DTO.AdminArea.DriveDTO>().ReverseMap();
        
        CreateMap<App.Public.DTO.v1.AdminArea.Comment, App.BLL.DTO.AdminArea.CommentDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.CustomerArea.Comment, App.BLL.DTO.AdminArea.CommentDTO>().ReverseMap();
        
        CreateMap<App.BLL.DTO.AdminArea.PhotoDTO, App.DAL.DTO.AdminArea.PhotoDTO>().ReverseMap();
        
    }
}