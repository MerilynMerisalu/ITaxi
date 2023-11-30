using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete vehicle mark view model
/// </summary>
public class DetailsDeleteVehicleMarkViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Vehicle mark ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle mark name
    /// </summary>
    [Display(ResourceType = typeof(VehicleMark), Name = nameof(VehicleMarkName))]
    public string VehicleMarkName { get; set; } = default!;
}