using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateAdminViewModel
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = "Gender")]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    [Display(ResourceType = typeof(Admin), Name = "DateOfBirth")]
    [DataType(DataType.Date)]

#warning Ask if it can be refactoried to use partial view
    public string DateOfBirth { get; set; } = default!;

    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [Display(ResourceType = typeof(Admin), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    [Display(ResourceType = typeof(Admin), Name = "City")]
    public Guid CityId { get; set; }

    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.EmailAddress)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1)]
    [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
    [Display(ResourceType = typeof(Common), Name = "Email")]

    public string Email { get; set; } = default!;

    [Required]
    [StringLength(100, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = nameof(Password))]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = "ConfirmPassword")]
    [Compare("Password", ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ConfirmPassword")]
    public string ConfirmPassword { get; set; } = default!;


    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]
    public IsActive IsActive { get; set; }

    public SelectList? Cities { get; set; }
}