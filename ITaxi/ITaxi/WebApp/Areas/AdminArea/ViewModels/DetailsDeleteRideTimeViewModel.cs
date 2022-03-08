using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteRideTimeViewModel
{
    public Guid Id { get; set; }

    public string Schedule { get; set; } = default!;

    [DisplayName("Shift Duration Time")]
    public string ShiftDurationTime { get; set; } = default!;

    [DisplayName("Ride Time")] 
    public string RideTime { get; set; } = default!;

    [DisplayName("Is Taken") ]
    public bool IsTaken { get; set; }


}