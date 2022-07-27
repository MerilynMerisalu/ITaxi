using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteRideTimeViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(RideTime),
        Name = nameof(Driver))]
    public string Driver { get; set; } = default!;


    [Display(ResourceType = typeof(RideTime),
        Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    [Display(ResourceType = typeof(RideTime),
        Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    [Display(ResourceType = typeof(RideTime),
        Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }
}