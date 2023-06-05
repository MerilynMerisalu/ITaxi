using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete city view model
/// </summary>
public class DetailsDeleteCityViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// City id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// County name
    /// </summary>
    [Display(ResourceType = typeof(City), Name = "CountyName")]
    public string CountyName { get; set; } = default!;

    /// <summary>
    /// City name
    /// </summary>
    [Display(ResourceType = typeof(City), Name = "CityName")]
    public string CityName { get; set; } = default!;
    
}