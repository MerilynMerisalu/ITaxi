namespace WebApp.DTO.Identity;

public class RefreshTokenModelDTO
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}