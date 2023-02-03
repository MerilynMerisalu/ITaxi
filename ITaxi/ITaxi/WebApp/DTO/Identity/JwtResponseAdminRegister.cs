

using App.BLL.DTO.AdminArea;

namespace WebApp.DTO.Identity;

public class JwtResponseAdminRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public AdminDTO AdminDto { get; set; } 
}