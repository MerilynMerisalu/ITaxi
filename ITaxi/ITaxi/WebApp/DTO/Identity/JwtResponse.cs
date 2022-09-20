namespace WebApp.DTO.Identity;

public class JwtResponse
{
    public string Token { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}