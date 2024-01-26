using Public.App.DTO.v1.DriverArea;

namespace Webapp.ViewModels
{
    public class CreateRideTimeViewModel
    {
        public Guid? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        public string? RideTimeForValidation { get; set; }
        public IEnumerable<string?> SelectedRideTimes { get; set; } = Array.Empty<string>();
        public IEnumerable<DateTime?> SelectedRideTimesDateTime { get; set; } = Array.Empty<DateTime?>();
        public bool IsTaken { get; set; }


    }
}
