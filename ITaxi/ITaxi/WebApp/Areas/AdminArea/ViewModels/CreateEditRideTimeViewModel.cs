using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditRideTimeViewModel
{
    public Guid ScheduleId { get; set; }

    [DisplayFormat(DataFormatString ="{0:hh:mm}" )]
    public SelectList? RideTimes { get; set; }

    public ICollection<DateTime>? SelectedRideTimes { get; set; }
    
    public SelectList? Schedules { get; set; }

   
    
    public bool IsTaken { get; set; }
}