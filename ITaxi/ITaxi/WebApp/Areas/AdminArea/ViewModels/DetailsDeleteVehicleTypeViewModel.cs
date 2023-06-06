using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete vehicle type view model
/// </summary>
public class DetailsDeleteVehicleTypeViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Vehicle type id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle type name
    /// </summary>
    [Display(ResourceType = typeof(VehicleType), Name = nameof(VehicleTypeName))]
    public string VehicleTypeName { get; set; } = default!;
}