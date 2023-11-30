using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete driver view model
/// </summary>
public class DetailsDeleteDriverViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Driver's id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver's first name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Driver's last name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "LastName")]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Driver's last and first name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "LastAndFirstName")]
    public string LastAndFirstName { get; set; } = default!;

    /// <summary>
    /// Driver's gender
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Driver's date of birth
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:d}")]
    [Display(ResourceType = typeof(Driver), Name = "DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Driver's personal identifier
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    /// <summary>
    /// Driver's driver license number
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;

    /// <summary>
    /// Driver's driver license category names
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]
    public string? DriverLicenseCategoryNames { get; set; }

    /// <summary>
    /// Driver's driver license expiry date
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DriverLicenseExpiryDate { get; set; }
    
    /// <summary>
    /// City name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "City")]
    public string CityName { get; set; } = default!;

    /// <summary>
    /// Driver's address
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Driver's phone number
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "PhoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Drivers email address
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "Email")]
    public string EmailAddress { get; set; } = default!;
}