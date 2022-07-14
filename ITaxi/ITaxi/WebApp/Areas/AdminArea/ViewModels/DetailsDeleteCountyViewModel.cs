
using System.ComponentModel.DataAnnotations;


namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteCountyViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County),
        Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;
    
}