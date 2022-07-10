using System.ComponentModel;
using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteScheduleViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName(nameof(Driver))] 
    public string DriversFullName { get; set; } = default!;

    [DisplayName(nameof(Vehicle))] 
    public string VehicleIdentifier { get; set; } = default!;

    [DisplayName("Start Date and Time")] 
    public string StartDateAndTime { get; set; } = default!;

    [DisplayName("End Date and Time")] 
    public string EndDateAndTime { get; set; } = default!;
}