using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDisabilityTypeViewModel
{
    public Guid Id { get; set; }

    [DisplayName("Disability Type")]
    public string DisabilityType { get; set; } = default!;
}