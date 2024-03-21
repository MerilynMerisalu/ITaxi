

using App.BLL.DTO.AdminArea;

namespace WebApp.DTO.Identity;

/// <summary>
/// Jwt Response admin register
/// </summary>
public class JwtResponseAdminRegister
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = default!;
    
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;

    /// <summary>
    /// Admin DTO object
    /// </summary>
    public AdminDTO AdminDto { get; set; } = default!;
}