using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateDriverViewModel
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "LastName")]
    public string LastName { get; set; } = default!;

    [EnumDataType(typeof(Gender))]
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Driver), Name = "DateOfBirth")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
#warning Ask if it can be refactoried to use partial view
    public string DateOfBirth { get; set; } = default!;


    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver),
        Name = nameof(PersonalIdentifier))]
    public string? PersonalIdentifier { get; set; }

    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common)
        , ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver),
        Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;


    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]

    public SelectList? DriverLicenseCategories { get; set; }

    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]
    public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
    public string DriverLicenseExpiryDate { get; set; } = default!;

    public SelectList? Cities { get; set; }

    [Display(ResourceType = typeof(Driver), Name = "City")]
    public Guid CityId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(30, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]

    public bool IsActive { get; set; }
}