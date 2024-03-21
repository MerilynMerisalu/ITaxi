using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create ride time view model
/// </summary>
public class CreateRideTimeViewModel
{
    /// <summary>
    /// Schedule id for ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    /// <summary>
    /// Driver id for ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "Driver")]
    public Guid DriverId { get; set; }

    /// <summary>
    /// List of ride times
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:t}")]
    [DataType(DataType.Time)]
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    /// <summary>
    /// Selected ride times
    /// </summary>
    [RequiredAtLeastOneSelection(ErrorMessage = "At least 1 Ride Time must be selected")]
    public ICollection<DateTime>? SelectedRideTimes { get; set; }

    /// <summary>
    /// List of schedules
    /// </summary>
    public SelectList? Schedules { get; set; }

    /// <summary>
    /// Boolean is ride time taken
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }

    /// <summary>
    /// List of drivers
    /// </summary>
    public SelectList? Drivers { get; set; }
}