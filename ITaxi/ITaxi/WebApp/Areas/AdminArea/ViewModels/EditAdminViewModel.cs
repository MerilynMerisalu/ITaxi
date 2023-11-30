using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Edit admin view model
/// </summary>
public class EditAdminViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Admin first name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Admin last name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Admin gender
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    /// <summary>
    /// Admin date of birth
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = "DateOfBirth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Admin personal identifier
    /// </summary>
    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [Display(ResourceType = typeof(Admin), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    /// <summary>
    /// City id
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = "City")]
    public Guid CityId { get; set; }

    /// <summary>
    /// Admin address
    /// </summary>
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Admin phone number
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Admin email
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
    /// Boolean is admin active
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]
    public bool IsActive { get; set; }

    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }
}