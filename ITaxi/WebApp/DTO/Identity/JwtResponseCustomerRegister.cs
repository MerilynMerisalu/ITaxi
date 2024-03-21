using App.BLL.DTO.AdminArea;

namespace WebApp.DTO.Identity;

/// <summary>
/// Jwt Response Customer Register
/// </summary>
public class JwtResponseCustomerRegister
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
    /// Customer's first name
    /// </summary>
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// Customer's last name
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Customer's first anf last name
    /// </summary>
    public string FirstAndLastName => $"{FirstName} {LastName}";

    /// <summary>
    /// Customer's last and first name
    /// </summary>
    public string LastAndFirstName => $"{LastName} {FirstName}";
    
    /// <summary>
    /// Role names
    /// </summary>
    public string[] RoleNames { get; set; } = default!;
    
    /// <summary>
    /// Customer object
    /// </summary>
    public CustomerDTO? CustomerDTO { get; set; }
}