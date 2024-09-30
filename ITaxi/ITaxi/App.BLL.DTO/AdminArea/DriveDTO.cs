using System.ComponentModel.DataAnnotations;

using App.Enum.Enum;
using App.Resources.Areas.App.Domain.DriverArea;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class DriveDTO : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "JobTitle")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "JobTitle")]
    public DriverDTO? Driver { get; set; }

    public BookingDTO? Booking { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "CommentText")]
    public CommentDTO? Comment { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "AcceptedDateAndTime")]

    public DateTime DriveAcceptedDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "DeclineDateAndTime")]

    public DateTime DriveDeclineDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "InProgressDateAndTime")]

    public DateTime DriveStartDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = "StatusOfDrive")]
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
    public string DriveDescription { get; internal set; } = default!;

    public string? DriveAcceptInformation => $"{StatusOfDrive} {AcceptedBy} {DriveAcceptedDateAndTime}";
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Drive), Name = nameof(CustomerFullNameAndPhoneNumber))]
    public string? CustomerFullNameAndPhoneNumber => $"{Booking!.Customer!.AppUser!.FirstAndLastName}; {Drive.CustomerPhoneNumber} {Booking.Customer.AppUser.PhoneNumber}";
}