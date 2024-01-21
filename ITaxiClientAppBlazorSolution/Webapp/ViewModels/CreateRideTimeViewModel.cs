using Public.App.DTO.v1.DriverArea;

namespace Webapp.ViewModels
{
    public class CreateRideTimeViewModel
    {
        public Guid ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public RideTime? RideTimeForValidation { get; set; }
        public IEnumerable<RideTime?> SelectedRideTimes { get; set; } = Array.Empty<RideTime>();


    }
}
