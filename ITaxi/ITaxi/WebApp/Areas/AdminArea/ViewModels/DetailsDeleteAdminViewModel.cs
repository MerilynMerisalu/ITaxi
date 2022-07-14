
using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Enum;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteAdminViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(FirstName))]

    public string FirstName { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(LastName))]

    public string LastName { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(LastAndFirstName))]

    public string LastAndFirstName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Gender))]
    public Gender Gender { get; set; }
    
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "DateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(PersonalIdentifier))]
    public string PersonalIdentifier { get; set; } = default!;
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(City))]

    public City? City { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = "PhoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Base.Resources.Common), Name = "IsActive")]
    public bool IsActive { get; set; }

    [Display(ResourceType = typeof(Base.Resources.Common), Name = "Email")]

    public string Email { get; set; } = default!;

}