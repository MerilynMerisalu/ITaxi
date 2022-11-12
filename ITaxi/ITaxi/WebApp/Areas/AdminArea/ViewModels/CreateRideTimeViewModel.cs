using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateRideTimeViewModel
{
    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = "Driver")]
    public Guid DriverId { get; set; }

    [DisplayFormat(DataFormatString = "{0:t}")]
    [DataType(DataType.Time)]
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    [RequiredAtLeastOneSelection(ErrorMessage = "At least 1 Ride Time must be selected")]
    public ICollection<DateTime>? SelectedRideTimes { get; set; }

    public SelectList? Schedules { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }

    public SelectList? Drivers { get; set; }
}