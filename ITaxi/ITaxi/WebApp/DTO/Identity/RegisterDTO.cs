using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;

namespace WebApp.DTO.Identity;

public class RegisterDTO
{
    public string FirstName { get; set; } = default!;

    [Required()]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    
    public string LastName { get; set; } = default!;
    
    
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.DateTime)]

    public string DateOfBirth { get; set; } = default!;
    
    [Required()]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    
    public  string PhoneNumber { get; set; } = default!;

    [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid email address length")]
    
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}