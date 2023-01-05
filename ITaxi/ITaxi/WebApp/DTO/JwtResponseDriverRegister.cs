using App.DAL.DTO.AdminArea;
using DriverAndDriverLicenseCategoryDTO = App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO;
using DriverDTO = App.BLL.DTO.AdminArea.DriverDTO;

namespace WebApp.DTO;

public class JwtResponseDriverRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public DriverDTO DriverDTO { get; set; }
    public DriverAndDriverLicenseCategoryDTO DriverAndDriverLicenseCategoryDTO { get; set; }
}