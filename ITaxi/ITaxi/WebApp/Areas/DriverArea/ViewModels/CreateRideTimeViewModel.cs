using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Create ride time view model
/// </summary>
public class CreateRideTimeViewModel
{
    /// <summary>
    /// Schedule id
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }
    
    /// <summary>
    /// List of ride times
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:t}")]
    [DataType(DataType.Time)]
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    /// <summary>
    /// List of selected ride times
    /// </summary>
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
}