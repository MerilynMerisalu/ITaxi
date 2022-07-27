using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

public class DetailsDeleteScheduleViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Schedule), Name = "Vehicle")]
    public string VehicleIdentifier { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = "ShiftStartDateAndTime")]
    public string StartDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(Schedule), Name = "ShiftEndDateAndTime")]
    public string EndDateAndTime { get; set; } = default!;
}