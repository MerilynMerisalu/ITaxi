using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditRideTimeViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = "Driver")]
    public Guid DriverId { get; set; }

    public SelectList? Drivers { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    public SelectList? Schedules { get; set; }


    [Display(ResourceType = typeof(RideTime), Name = "RideDateAndTime")]

    public string RideTime { get; set; } = default!;

    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}