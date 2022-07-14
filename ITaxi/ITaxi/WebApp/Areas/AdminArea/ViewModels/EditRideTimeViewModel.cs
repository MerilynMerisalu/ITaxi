using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using RideTime = App.Resources.Areas.App.Domain.AdminArea.RideTime;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditRideTimeViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    public SelectList? Schedules { get; set; }

    //[DataType(DataType.Time)]
    [Display(ResourceType = typeof(RideTime), Name = "RideDateAndTime")]
    //[DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = false)]
    public string RideTime { get; set; } = default!;
    
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}