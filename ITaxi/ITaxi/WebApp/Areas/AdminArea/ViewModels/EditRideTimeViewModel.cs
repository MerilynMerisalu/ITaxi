using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditRideTimeViewModel
{
    public Guid Id { get; set; }

    [DisplayName(nameof(Schedule))]
    public Guid ScheduleId { get; set; }

    public SelectList? Schedules { get; set; }

    [DataType(DataType.Time)]
    [DisplayName("Ride Time")]
    [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
    public string RideTime { get; set; } = default!;
    
    public SelectList? RideTimes { get; set; }
    [DisplayName("Is Taken")]
    public bool IsTaken { get; set; }
}