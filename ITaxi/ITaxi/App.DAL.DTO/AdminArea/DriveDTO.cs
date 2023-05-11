using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using Base.Domain;

namespace App.DAL.DTO.AdminArea;

public class DriveDTO: DomainEntityMetaId
{
    
    public Guid DriverId { get; set; }

    public DriverDTO? Driver { get; set; }

    public BookingDTO Booking { get; set; }
    
    public CommentDTO? Comment { get; set; }

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
    
    /*public string DriveAcceptedDateTimeDriverView => $"{DriveAcceptedDateAndTime:g}";
    public string DriveDeclinedDateTimeDriverView => $"{DriveDeclineDateAndTime:g}";
    public string DriveStartedDateTimeDriverView => $"{DriveStartDateAndTime:g}";
    public string DriveEndDateTimeDriverView => $"{DriveEndDateAndTime:g}";

    public string DriveDescription => $"{Booking!.PickUpDateAndTime:g} {Driver!.AppUser!.LastAndFirstName}";
    */
    

    //public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
}
