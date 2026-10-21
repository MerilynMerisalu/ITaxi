using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.Identity.Pages.Account;
using Base.Resources;

namespace WebApp.DTO.Identity;

/// <summary>
/// Register DTO for the all the user types
/// </summary>
public class RegisterDTO
{
    /// <summary>
    /// User's first name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.StringLengthAttributeErrorMessage))]

    public string FirstName { get; set; } = default!;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.StringLengthAttributeErrorMessage))]
    public string LastName { get; set; } = default!;
    
    /// <summary>
    /// User's gender
    /// </summary>
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    /// <summary>
    /// User's date of birth
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [DataType(DataType.Date)]
    public string DateOfBirth { get; set; } = default!;

    /// <summary>
    /// User's phone number
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.StringLengthAttributeErrorMessage))]
    public  string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// User's email
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof( Common.RequiredAttributeErrorMessage))]
    [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageEmail))]
    [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.StringLengthAttributeErrorMessage))]
    public string Email { get; set; } = default!;
    /// <summary>
    /// User's password
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(100, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(AdminRegister), Name = nameof(Password))]
    public string Password { get; set; } = default!;
}