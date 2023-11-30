using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Edit schedule view model
/// </summary>
public class EditScheduleViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver id
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "Driver")]
    public Guid DriverId { get; set; }

    /// <summary>
    /// Vehicle id
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]
    public Guid VehicleId { get; set; }
    
    /// <summary>
    /// Start date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    public DateTime StartDateAndTime { get; set; }

    /// <summary>
    /// End date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    public DateTime EndDateAndTime { get; set; }

    /// <summary>
    /// List of vehicles
    /// </summary>
    public SelectList? Vehicles { get; set; }

    /// <summary>
    /// List of drivers
    /// </summary>
    public SelectList? Drivers { get; set; }
}