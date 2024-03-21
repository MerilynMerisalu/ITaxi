using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Details delete schedule view model
/// </summary>
public class DetailsDeleteScheduleViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle identifier
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]
    public string VehicleIdentifier { get; set; } = default!;

    /// <summary>
    /// Schedule start date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    public string StartDateAndTime { get; set; } = default!;

    /// <summary>
    /// Schedule end date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    public string EndDateAndTime { get; set; } = default!;
}