using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Edit ride time view model
/// </summary>
public class EditRideTimeViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver id
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "Driver")]
    public Guid DriverId { get; set; }

    /// <summary>
    /// List of drivers
    /// </summary>
    public SelectList? Drivers { get; set; }

    /// <summary>
    /// Schedule id
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    /// <summary>
    /// List of schedules
    /// </summary>
    public SelectList? Schedules { get; set; }
    
    /// <summary>
    /// Ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    /// <summary>
    /// List of ride times
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    /// <summary>
    /// Boolean is ride time taken
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}