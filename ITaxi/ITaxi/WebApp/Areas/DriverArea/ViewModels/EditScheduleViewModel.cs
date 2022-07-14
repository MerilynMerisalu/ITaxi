using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class EditScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftStartDateAndTime")]
    
    public DateTime StartDateAndTime { get; set; } = default!;
    

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftEndDateAndTime")]
    
    public DateTime EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }

}