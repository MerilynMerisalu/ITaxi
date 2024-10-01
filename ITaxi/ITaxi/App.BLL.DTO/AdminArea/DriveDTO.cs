using System.ComponentModel.DataAnnotations;

using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class DriveDTO : DomainEntityMetaId
{
    public Guid ScheduleId { get; set; }
    [Display(ResourceType = typeof(Drive), Name = nameof(Schedule))]
    public ScheduleDTO? Schedule { get; set; }
    //[Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public Guid DriverId { get; set; }

    //[Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public DriverDTO? Driver { get; set; }

    public BookingDTO? Booking { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "CommentText")]
    public CommentDTO? Comment { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "AcceptedDateAndTime")]

    public DateTime DriveAcceptedDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "DeclineDateAndTime")]

    public DateTime DriveDeclineDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "InProgressDateAndTime")]

    public DateTime DriveStartDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "StatusOfDrive")]
    public StatusOfDrive StatusOfDrive { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "FinishedDateAndTime")]
    public DateTime DriveEndDateAndTime { get; set; }

    public bool IsDriveAccepted { get; set; }

    public string? AcceptedBy { get; set; }

    public bool IsDriveDeclined { get; set; }

    public bool IsDriveStarted { get; set; }

    public bool IsDriveFinished { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "AcceptedDateAndTime")]
    public string DriveAcceptedDateTimeDriverView => $"{DriveAcceptedDateAndTime:g}";

    [Display(ResourceType = typeof(Drive), Name = "DeclineDateAndTime")]
    public string DriveDeclinedDateTimeDriverView => $"{DriveDeclineDateAndTime:g}";

    [Display(ResourceType = typeof(Drive), Name = "InProgress")]
    public string DriveStartedDateTimeDriverView => $"{DriveStartDateAndTime:g}";

    [Display(ResourceType = typeof(Drive), Name = "Finished")]
    public string DriveEndDateTimeDriverView => $"{DriveEndDateAndTime:g}";
    public string DriveDescription { get; internal set; } = default!;

    public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
    [Display(ResourceType = typeof(Drive), Name = nameof(CustomerInfo))]
    public string? CustomerInfo => $"{Booking!.Customer!.AppUser!.FirstAndLastName};" +
        $" {Drive.PhoneNumber} {Booking.Customer.AppUser.PhoneNumber}";
    [Display(ResourceType = typeof(Drive), Name = nameof(DriverInfo))]    
    public string? DriverInfo => $"{Driver!.AppUser.FirstAndLastName};" +
                                                     $" {Drive.PhoneNumber} {Driver.AppUser.PhoneNumber}";
}