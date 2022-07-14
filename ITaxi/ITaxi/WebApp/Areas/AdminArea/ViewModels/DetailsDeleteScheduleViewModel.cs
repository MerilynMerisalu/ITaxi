using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Schedule = App.Resources.Areas.App.Domain.AdminArea.Schedule;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteScheduleViewModel:AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(Schedule), Name = "Driver")] 
    public string DriversFullName { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]  
    public string VehicleIdentifier { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]  
    public string StartDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")] 
    public string EndDateAndTime { get; set; } = default!;
}