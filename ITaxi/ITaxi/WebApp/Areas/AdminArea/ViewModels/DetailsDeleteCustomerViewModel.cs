using System.ComponentModel;
using WebApp.Models.Enum;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCustomerViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName("Disability Type")]

    public string DisabilityTypeName { get; set; } = default!;
    
    [DisplayName("Last And First Name")]

    public string LastAndFirstName { get; set; } = default!;

    [DisplayName("Date of Birth")]
    public string DateOfBirth { get; set; } = default!;

    public Gender Gender { get; set; }

    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; } = default!;

    public string Email { get; set; } = default!;




}