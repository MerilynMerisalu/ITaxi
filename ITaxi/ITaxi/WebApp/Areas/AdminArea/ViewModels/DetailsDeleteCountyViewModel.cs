using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCountyViewModel
{
    public Guid Id { get; set; }
    [DisplayName("County Name")]
    public string CountyName { get; set; } = default!;
}