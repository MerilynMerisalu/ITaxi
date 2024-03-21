using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete vehicle model view model
/// </summary>
public class DetailsDeleteVehicleModelViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Vehicle model id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle model name
    /// </summary>
    [Display(ResourceType = typeof(VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    /// <summary>
    /// Vehicle mark id
    /// </summary>
    public Guid VehicleMarkId { get; set; }

    /// <summary>
    /// Vehicle mark name
    /// </summary>
    [Display(ResourceType = typeof(VehicleMark), Name = nameof(VehicleMarkName))]
    public string VehicleMarkName { get; set; } = default!;
}