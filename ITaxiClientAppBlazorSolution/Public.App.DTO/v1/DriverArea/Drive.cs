using ITaxi.Enum.Enum;
using Public.App.DTO.v1.CustomerArea;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.App.DTO.v1.DriverArea
{
    public class Drive: Entity
    {
        
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver), Name = "JobTitle")]
        public Guid DriverId { get; set; }
        public Booking? Booking { get; set; }


        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = nameof(Comment))]
        public Comment? Comment { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = "AcceptedDateAndTime")]

        public DateTime DriveAcceptedDateAndTime { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = "DeclineDateAndTime")]

        public DateTime DriveDeclineDateAndTime { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = "InProgressDateAndTime")]

        public DateTime DriveStartDateAndTime { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = "DriveStatus")]
        public StatusOfDrive StatusOfDrive { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Drive), Name = "FinishedDateAndTime")]
        public DateTime DriveEndDateAndTime { get; set; }

        public bool IsDriveAccepted { get; set; }
        public string? AcceptedBy { get; set; }
        public bool IsDriveDeclined { get; set; }
        public bool IsDriveStarted { get; set; }

        public bool IsDriveFinished { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Drive), Name = "AcceptedDateAndTime")]
        public string DriveAcceptedDateTimeDriverView => $"{DriveAcceptedDateAndTime:g}";

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Drive), Name = "DeclineDateAndTime")]
        public string DriveDeclinedDateTimeDriverView => $"{DriveDeclineDateAndTime:g}";

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Drive), Name = "InProgress")]
        public string DriveStartedDateTimeDriverView => $"{DriveStartDateAndTime:g}";

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Drive), Name = "Finished")]

        public string DriveEndDateTimeDriverView => $"{DriveEndDateAndTime:g}";

        public string DriveDescription =>
            $"{Booking!.PickUpDateAndTime:g} ";
        // $"- {AppUser!.LastAndFirstName}";

        public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
    }
}
