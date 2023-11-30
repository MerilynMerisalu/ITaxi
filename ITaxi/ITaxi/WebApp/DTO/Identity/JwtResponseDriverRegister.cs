using App.Public.DTO.v1.Identity;
using DriverAndDriverLicenseCategoryDTO = App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO;
using DriverDTO = App.BLL.DTO.AdminArea.DriverDTO;

namespace WebApp.DTO.Identity;

/// <summary>
/// Jet response driver register
/// </summary>
public class JwtResponseDriverRegister
{
    /// <summary>
    /// App user Id for the driver user
    /// </summary>
    public Guid AppUserId { get; set; }
    
    /// <summary>
    /// App user for the driver user
    /// </summary>
    public AppUser? AppUser { get; set; }
    
    /// <summary>
    /// Token for the app user
    /// </summary>
    public string Token { get; set; } = default!;
    
    /// <summary>
    /// Refresh token for the app user
    /// </summary>
    public string RefreshToken { get; set; } = default!;
    
    /// <summary>
    /// Driver first name
    /// </summary>
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// Driver last name
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Driver first and last name
    /// </summary>
    public string FirstAndLastName => $"{FirstName} {LastName}";

    /// <summary>
    /// Driver last and first name
    /// </summary>
    public string LastAndFirstName => $"{LastName} {FirstName}";
    
    /// <summary>
    /// Names of roles
    /// </summary>
    public string[] RoleNames { get; set; } = default!;

    /// <summary>
    /// Driver object 
    /// </summary>
    public DriverDTO? DriverDTO { get; set; }
    
    /// <summary>
    /// Driver and driver license category object
    /// </summary>
    public DriverAndDriverLicenseCategoryDTO? DriverAndDriverLicenseCategoryDTO { get; set; }
}