using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditRideTimeViewModel
{
    public Guid ScheduleId { get; set; }

    [DisplayFormat(DataFormatString ="{0:hh:mm}" )]
    public SelectList? RideTimes { get; set; }
    
    public SelectList? Schedules { get; set; }

    public DateTime RideDateTime { get; set; }
    
    public bool IsTaken { get; set; }
}