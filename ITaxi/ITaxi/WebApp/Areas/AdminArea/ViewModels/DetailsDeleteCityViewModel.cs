using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCityViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(City), Name = "CountyName")]
    public string CountyName { get; set; } = default!;

    [Display(ResourceType = typeof(City), Name = "CityName")]
    public string CityName { get; set; } = default!;
    
}