using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.DriverArea.ViewModels;

public class DetailsDeleteRideTimeViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.RideTime),
        Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.RideTime),
        Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.RideTime),
        Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }

}