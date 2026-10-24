using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Google.Apis.PeopleService.v1.Data;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete admin view model
/// </summary>
public class DetailsDeleteAdminViewModel : AdminAreaBaseViewModel
{
    

    /// <summary>
    /// Admin first name
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Admin last name
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Admin last and first name
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = nameof(LastAndFirstName))]
    public string LastAndFirstName { get; set; } = default!;

    /// <summary>
    /// Admin gender
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = nameof(Gender))]
    public App.Enum.Enum.Gender Gender { get; set; }

    /// <summary>
    /// Admin date of birth
    /// </summary>
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(Admin), Name = "DateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Admin personal identifier
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = nameof(PersonalIdentifier))]
    public string? PersonalIdentifier { get; set; }
    /// <summary>
    /// Admin's country
    /// </summary>
    [Display(ResourceType = typeof(AppUser), Name = "Country")]
    public string CountryName { get; set; } = default!;

    /// <summary>
    /// Admin's county
    /// </summary>
    [Display(ResourceType = typeof(AppUser), Name = "County")]
    public string CountyName { get; set; } = default!;
    /// <summary>
    /// Admin's municipality
    /// </summary>
    [Display(ResourceType = typeof(AppUser), Name = "Municipality")]
    public string MunicipalityName { get; set; } = default!;
    /// <summary>
    /// Admin's locality
    /// </summary>
    [Display(ResourceType = typeof(AppUser), Name = "Locality")]
    public string LocalityName { get; set; } = default!;
    /// <summary>
    /// Admin's residence address
    /// </summary>
    [Display(ResourceType = typeof(Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Admin phone number
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "PhoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Boolean is admin active
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "IsActive")]
    public bool IsActive { get; set; } = default;

    /// <summary>
    /// Admin email
    /// </summary>
    [Display(ResourceType = typeof(Common), Name = "Email")]
    public string Email { get; set; } = default!;
}