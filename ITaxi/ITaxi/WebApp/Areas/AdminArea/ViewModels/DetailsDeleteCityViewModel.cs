
using System.ComponentModel.DataAnnotations;


namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCityViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City), Name = "CountyName")]
    public string CountyName { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City), Name = "CityName")]
    public string CityName { get; set; } = default!;

    

}