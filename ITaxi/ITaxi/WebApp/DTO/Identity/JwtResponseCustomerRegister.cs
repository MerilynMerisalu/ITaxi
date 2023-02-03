using App.Domain.DTO;
using App.Domain.DTO.AdminArea;
using CustomerDTO = App.BLL.DTO.AdminArea.CustomerDTO;

namespace WebApp.DTO.Identity;

public class JwtResponseCustomerRegister
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public CustomerDTO CustomerDTO { get; set; }
}