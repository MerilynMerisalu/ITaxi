using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;

namespace App.Domain;

public class Drive : DomainEntityMetaId
{
    
    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }

    public Booking? Booking { get; set; }
    public Comment? Comment { get; set; }
    public DateTime DriveAcceptedDateAndTime { get; set; }
    
    public DateTime DriveDeclineDateAndTime { get; set; }
    
    public DateTime DriveStartDateAndTime { get; set; }

    public StatusOfDrive StatusOfDrive { get; set; }
    
    public DateTime DriveEndDateAndTime { get; set; }

    public bool IsDriveAccepted { get; set; }

    public string? AcceptedBy { get; set; }

    public bool IsDriveDeclined { get; set; }

    public bool IsDriveStarted { get; set; }

    public bool IsDriveFinished { get; set; }
    public string DriveAcceptedDateTimeDriverView => $"{DriveAcceptedDateAndTime:g}";
    public string DriveDeclinedDateTimeDriverView => $"{DriveDeclineDateAndTime:g}";
    
    public string DriveStartedDateTimeDriverView => $"{DriveStartDateAndTime:g}";
    public string DriveEndDateTimeDriverView => $"{DriveEndDateAndTime:g}";
    public string DriveDescription
    {
        get =>
            $"{Booking!.PickUpDateAndTime:g} " +
            $"- {Driver!.AppUser!.LastAndFirstName}";
    }

    public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
}