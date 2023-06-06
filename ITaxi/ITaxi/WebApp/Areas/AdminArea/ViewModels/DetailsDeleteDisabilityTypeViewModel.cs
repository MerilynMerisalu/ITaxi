using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete disability type view model
/// </summary>
public class DetailsDeleteDisabilityTypeViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Disability type id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Disability type
    /// </summary>
    [Display(ResourceType = typeof(DisabilityType), Name = "DisabilityTypeName")]
    public string DisabilityType { get; set; } = default!;
}