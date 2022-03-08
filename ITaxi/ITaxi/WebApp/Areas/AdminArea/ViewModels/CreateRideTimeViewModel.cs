using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateRideTimeViewModel
{
    [DisplayName(nameof(Schedule))]
    public Guid ScheduleId { get; set; }

    [DisplayFormat(DataFormatString ="{0:hh:mm}" )]
    [DataType(DataType.Time)]
    [DisplayName("Ride Times")]
    public SelectList? RideTimes { get; set; }
    
    public ICollection<DateTime>? SelectedRideTimes { get; set; }
    
    public SelectList? Schedules { get; set; }
    [DisplayName("Is Taken")]
    public bool IsTaken { get; set; }
}