using Public.App.DTO.v1.DriverArea;

namespace Webapp.ViewModels
{
    public class CreateRideTimeViewModel
    {
        public Guid ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }



    }
}
