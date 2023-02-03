

using App.BLL.DTO.AdminArea;

namespace WebApp.DTO.Identity;

public class JwtResponseCustomerRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public CustomerDTO CustomerDTO { get; set; }
}