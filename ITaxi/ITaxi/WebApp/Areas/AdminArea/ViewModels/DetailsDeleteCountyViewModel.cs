using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete county view model
/// </summary>
public class DetailsDeleteCountyViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// County id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// County name
    /// </summary>
    [Display(ResourceType = typeof(County), Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;
    
    /// <summary>
    /// County name
    /// </summary>
    [Display(ResourceType = typeof(County), Name = "Country")]
    public string CountryName { get; set; } = default!;
    
}