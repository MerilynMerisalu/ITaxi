using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCityViewModel
{
    public Guid Id { get; set; }
    [DisplayName("County Name")]
    public string CountyName { get; set; } = default!;
    [DisplayName("City Name")]
    public string CityName { get; set; } = default!;
    
    public string Address { get; set; } = default!;
}