using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Create schedule view model
/// </summary>
public class CreateScheduleViewModel
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
    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    public string StartDateAndTime { get; set; } = default!;

    /// <summary>
    /// Schedule end date and time
    /// </summary>
    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string EndDateAndTime { get; set; } = default!;

    /// <summary>
    /// List of vehicles
    /// </summary>
    public SelectList? Vehicles { get; set; }
}