using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;

namespace App.Domain;

public class Drive : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "JobTitle")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "JobTitle")]
    public Driver? Driver { get; set; }

    public Booking? Booking { get; set; }


    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = nameof(Comment))]
    public Comment? Comment { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "AcceptedDateAndTime")]
    
    public DateTime DriveAcceptedDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "DeclineDateAndTime")]

    public DateTime DriveDeclineDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "InProgressDateAndTime")]
    
    
    public DateTime DriveStartDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "DriveStatus")]
    public StatusOfDrive StatusOfDrive { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "FinishedDateAndTime")]
    public DateTime DriveEndDateAndTime { get; set; }

    public bool IsDriveAccepted { get; set; }

    public string? AcceptedBy { get; set; }

    public bool IsDriveDeclined { get; set; }

    public bool IsDriveStarted { get; set; }

    public bool IsDriveFinished { get; set; }
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Drive), Name = "AcceptedDateAndTime")]
    public string DriveAcceptedDateTimeDriverView => $"{DriveAcceptedDateAndTime:g}";
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Drive), Name = "DeclineDateAndTime")]
    public string DriveDeclinedDateTimeDriverView => $"{DriveDeclineDateAndTime:g}";
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Drive), Name = "InProgress")]
    public string DriveStartedDateTimeDriverView => $"{DriveStartDateAndTime:g}";
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Drive), Name = "Finished")]
    public string DriveEndDateTimeDriverView => $"{DriveEndDateAndTime:g}";
    public string DriveDescription
    {
        get =>
            $"{Booking!.PickUpDateAndTime:g} " +
            $"- {Driver!.AppUser!.LastAndFirstName}";

       
    }

    public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
}