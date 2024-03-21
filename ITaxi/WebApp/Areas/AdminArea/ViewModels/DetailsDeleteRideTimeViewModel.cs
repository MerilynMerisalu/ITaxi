using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete ride time view model
/// </summary>
public class DetailsDeleteRideTimeViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Ride time id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(Driver))]
    public string Driver { get; set; } = default!;
    
    /// <summary>
    /// Schedule
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    /// <summary>
    /// Ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    /// <summary>
    /// Boolean is ride time taken
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}