using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete vehicle view model
/// </summary>
public class DetailsDeleteVehicleViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Driver full name
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = "Driver")]
    public string DriverFullName { get; set; } = default!;

    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// Vehicle mark
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleMark))]
    public string VehicleMark { get; set; } = default!;

    /// <summary>
    /// Vehicle model
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleModel))]
    public string VehicleModel { get; set; } = default!;

    /// <summary>
    /// Vehicle plate number
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehiclePlateNumber))]
    public string VehiclePlateNumber { get; set; } = default!;

    /// <summary>
    /// Vehicle manufacture year
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(ManufactureYear))]
    public int ManufactureYear { get; set; }

    /// <summary>
    /// Number of seats
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(NumberOfSeats))]
    public int NumberOfSeats { get; set; }

    /// <summary>
    /// Vehicle availability
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = nameof(VehicleAvailability))]
    public VehicleAvailability VehicleAvailability { get; set; }
}