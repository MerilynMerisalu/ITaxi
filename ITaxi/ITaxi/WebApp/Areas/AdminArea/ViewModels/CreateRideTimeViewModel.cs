using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using RideTime = App.Resources.Areas.App.Domain.AdminArea.RideTime;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateRideTimeViewModel
{
    [Display(ResourceType = typeof(RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [DisplayFormat(DataFormatString ="{0:t" )]
    [DataType(DataType.Time)]
    [Display(ResourceType = typeof(RideTime), Name = "RideTimeSelectListName")]
    public SelectList? RideTimes { get; set; }
    
    public ICollection<DateTime>? SelectedRideTimes { get; set; }
    
    public SelectList? Schedules { get; set; }
    [Display(ResourceType = typeof(RideTime), Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}