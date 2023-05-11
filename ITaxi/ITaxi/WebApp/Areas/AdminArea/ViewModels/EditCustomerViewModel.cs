using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditCustomerViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Customer), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Customer), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;


    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [EnumDataType(typeof(Gender))]
    [Display(ResourceType = typeof(Customer), Name = nameof(Gender))]
    public Gender Gender { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Customer), Name = nameof(DateOfBirth))]
    public DateTime DateOfBirth { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Customer), Name = "DisabilityType")]

    public Guid DisabilityTypeId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Customer), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
    [Display(ResourceType = typeof(Common), Name = nameof(Email))]
    public string Email { get; set; } = default!;

    /*[Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(100, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = nameof(Password))]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Common), Name = nameof(ConfirmPassword))]
    [Compare(nameof(Password), ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageComparePasswords")]
    public string ConfirmPassword { get; set; } = default!;*/

    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]

    public bool IsActive { get; set; }

    public SelectList? DisabilityTypes { get; set; }
}