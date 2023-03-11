using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Enum;
using App.Resources.Areas.Identity.Pages.Account;
using Base.Resources;

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
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(100, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(AdminRegister), Name = nameof(Password))]
    public string Password { get; set; } = default!;
}