using System.ComponentModel.DataAnnotations;
using WebApp.DTO.Identity;

namespace WebApp.DTO;

/// <summary>
/// Administrator registration DTO
/// </summary>
public class AdminRegistrationDTO: RegisterDTO
{
    
    /// <summary>
    /// Administrator personal identifier
    /// </summary>
    [StringLength(50)]
    public string? PersonalIdentifier { get; set; }
    
    /// <summary>
    /// City id for the administrator
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public Guid CityId { get; set; }
    
    /// <summary>
    /// Administrator address
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    [StringLength(72, MinimumLength = 2)]
    public string Address { get; set; } = default!;
}