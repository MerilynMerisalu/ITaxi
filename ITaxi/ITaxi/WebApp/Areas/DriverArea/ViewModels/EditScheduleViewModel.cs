using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Edit schedule view model
/// </summary>
public class EditScheduleViewModel
{
    /// <summary>
    /// Schedule id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle id
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]
    public Guid VehicleId { get; set; }
    
    /// <summary>
    /// Schedule start date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    public DateTime StartDateAndTime { get; set; } = default!;


    /// <summary>
    /// Schedule end date and time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    public DateTime EndDateAndTime { get; set; } = default!;

    /// <summary>
    /// List of vehicles
    /// </summary>
    public SelectList? Vehicles { get; set; }
}