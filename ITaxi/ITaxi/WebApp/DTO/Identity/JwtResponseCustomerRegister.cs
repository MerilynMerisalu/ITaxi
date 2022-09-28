using App.Domain.DTO;

namespace WebApp.DTO.Identity;

public class JwtResponseCustomerRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public CustomerDTO? CustomerDTO { get; set; }
}