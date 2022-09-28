using App.Domain.DTO;

namespace WebApp.DTO;

public class JwtResponseDriverRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public DriverDTO? DriverDTO { get; set; }
    public DriverAndDriverLicenseCategoryDTO? DriverAndDriverLicenseCategoryDTO { get; set; }
}