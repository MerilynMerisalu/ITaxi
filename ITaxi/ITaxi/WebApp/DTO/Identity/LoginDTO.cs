using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO.Identity;

public class LoginDTO
{
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid email address length")] 
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}