using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Details delete ride time view model
/// </summary>
public class DetailsDeleteRideTimeViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    [Display(ResourceType = typeof(RideTime),
        Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    /// <summary>
    /// Ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime),
        Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    /// <summary>
    /// Boolean is taken
    /// </summary>
    [Display(ResourceType = typeof(RideTime),
        Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}