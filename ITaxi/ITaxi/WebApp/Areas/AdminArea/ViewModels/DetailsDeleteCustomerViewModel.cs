using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete customer view model
/// </summary>
public class DetailsDeleteCustomerViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Customer id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Customer last and first name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(LastAndFirstName))]
    public string LastAndFirstName { get; set; } = default!;

    /// <summary>
    /// Customer last name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;
    
    /// <summary>
    /// Customer first name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// Disability type name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = "DisabilityType")]
    public string DisabilityTypeName { get; set; } = default!;

    /// <summary>
    /// Customer date of birth
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(DateOfBirth))]
    public string DateOfBirth { get; set; } = default!;

    /// <summary>
    /// Customer gender
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(Gender))]
    public Gender Gender { get; set; }

    /// <summary>
    /// Customer phone number
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Customer email
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = nameof(Email))]
    public string Email { get; set; } = default!;
}