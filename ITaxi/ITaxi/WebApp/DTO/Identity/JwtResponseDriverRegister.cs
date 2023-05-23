using App.DAL.DTO.AdminArea;
using App.Public.DTO.v1.Identity;
using DriverAndDriverLicenseCategoryDTO = App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO;
using DriverDTO = App.BLL.DTO.AdminArea.DriverDTO;

namespace WebApp.DTO;

public class JwtResponseDriverRegister
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string FirstAndLastName => $"{FirstName} {LastName}";

    public string LastAndFirstName => $"{LastName} {FirstName}";
    
    public string[] RoleNames { get; set; } = default!;

    public DriverDTO DriverDTO { get; set; }
    public DriverAndDriverLicenseCategoryDTO DriverAndDriverLicenseCategoryDTO { get; set; }
}