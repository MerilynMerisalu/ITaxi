using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Edit ride time view model
/// </summary>
public class EditRideTimeViewModel
{
    /// <summary>
    /// Ride time id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Ride time schedule id
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
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:t}")]
    public string RideTime { get; set; } = default!; 

    /// <summary>
    /// List of ride times
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    /// <summary>
    /// Boolean is taken for ride time
    /// </summary>
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}