using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO.Identity;

public class AdminRegistrationDTO: RegisterDTO
{
#warning Should there be a common DTO for all types of registrations
    
    [StringLength(50)]
    public string? PersonalIdentifier { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public Guid CityId { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(72, MinimumLength = 2)]
    public string Address { get; set; } = default!;
}