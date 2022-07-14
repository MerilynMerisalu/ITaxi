
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteRideTimeViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.RideTime),
        Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.RideTime),
        Name = "RideDateAndTime")]
    public string RideTime { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.RideTime),
        Name = nameof(IsTaken))]
    public bool IsTaken { get; set; }


}