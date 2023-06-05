using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;
/// <summary>
/// Create admin view model
/// </summary>
public class CreateAdminViewModel
{
    /// <summary>
    /// Create admin first name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Create admin last name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Create admin gender
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    /// <summary>
    /// Create admin date of birth
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = "DateOfBirth")]
    [DataType(DataType.Date)]
    public string DateOfBirth { get; set; } = default!;

    /// <summary>
    /// Create admin personal identifier
    /// </summary>
    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [Display(ResourceType = typeof(Admin), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    /// <summary>
    /// City id for create admin
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = "City")]
    public Guid CityId { get; set; }

    /// <summary>
    /// Create admin address
    /// </summary>
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Create admin phone number
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Create admin email
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.EmailAddress)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1)]
    [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
    [Display(ResourceType = typeof(Common), Name = "Email")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// Create admin password
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = nameof(Password))]
    public string Password { get; set; } = default!;

    /// <summary>
    /// Create admin confirm password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = "ConfirmPassword")]
    [Compare("Password", ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ConfirmPassword")]
    public string ConfirmPassword { get; set; } = default!;
    
    /// <summary>
    /// Is admin active
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]
    public IsActive IsActive { get; set; }

    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }
}