using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create driver view model
/// </summary>
public class CreateDriverViewModel
{
    /// <summary>
    /// Driver first name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Driver last name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "LastName")]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Driver gender
    /// </summary>
    [EnumDataType(typeof(Gender))]
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Driver date of birth
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Driver), Name = "DateOfBirth")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
    public string DateOfBirth { get; set; } = default!;
    
    /// <summary>
    /// Driver personal identifier
    /// </summary>
    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver),
        Name = nameof(PersonalIdentifier))]
    public string? PersonalIdentifier { get; set; }

    /// <summary>
    /// Driver driver license number
    /// </summary>
    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common)
        , ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver),
        Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;


    /// <summary>
    /// Driver license categories
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]
    public SelectList? DriverLicenseCategories { get; set; }

    /// <summary>
    /// Driver and driver license categories
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]
    public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

    /// <summary>
    /// Driver license expiry date
    /// </summary>
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
    public string DriverLicenseExpiryDate { get; set; } = default!;

    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }

    /// <summary>
    /// City id for create a driver
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "City")]
    public Guid CityId { get; set; }

    /// <summary>
    /// Driver address
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(30, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Driver), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Boolean is driver active
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]
    public bool IsActive { get; set; }
}