
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditScheduleViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Vehicle")]

    public Guid VehicleId { get; set; }


    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    #warning Incorrect datetime formatting
    public string StartDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    
    #warning Incorrect datetime formatting
    public string EndDateAndTime { get; set; } = default!;

    public SelectList? Vehicles { get; set; }

}