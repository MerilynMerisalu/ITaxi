using App.Domain.DTO;

namespace WebApp.DTO.Identity;

public class JwtResponseAdminRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    
}