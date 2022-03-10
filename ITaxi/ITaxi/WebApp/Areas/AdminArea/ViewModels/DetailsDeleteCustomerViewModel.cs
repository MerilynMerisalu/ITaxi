using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCustomerViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName("Disability Type")]

    public string DisabilityTypeName { get; set; } = default!;
}