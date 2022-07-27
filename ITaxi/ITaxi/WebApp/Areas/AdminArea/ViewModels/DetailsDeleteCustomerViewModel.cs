using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCustomerViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Customer), Name = nameof(LastAndFirstName))]
    public string LastAndFirstName { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;


    [Display(ResourceType = typeof(Customer), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;


    [Display(ResourceType = typeof(Customer), Name = "DisabilityType")]

    public string DisabilityTypeName { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = nameof(DateOfBirth))]
    public string DateOfBirth { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = nameof(Gender))]
    public Gender Gender { get; set; }

    [Display(ResourceType = typeof(Customer), Name = nameof(PhoneNumber))]
    public string PhoneNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Customer), Name = nameof(Email))]
    public string Email { get; set; } = default!;
}