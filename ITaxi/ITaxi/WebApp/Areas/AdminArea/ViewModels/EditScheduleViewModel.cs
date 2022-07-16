
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Driver")]

    public Guid DriverId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Vehicle")]
    public Guid VehicleId { get; set; }


    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    
    public DateTime StartDateAndTime { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftEndDateAndTime")]
    
    public DateTime EndDateAndTime { get; set; }

    public SelectList? Vehicles { get; set; }

    public SelectList? Drivers { get; set; }

}