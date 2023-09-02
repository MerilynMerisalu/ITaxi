using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;
/// <summary>
/// Details delete country view model
/// </summary>
public class DetailsDeleteCountryViewModel: AdminAreaBaseViewModel
{
    /// <summary>
    /// Country id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    [Display(ResourceType = typeof(Country), Name = nameof(CountryName))]
    public string CountryName { get; set; } = default!;
}