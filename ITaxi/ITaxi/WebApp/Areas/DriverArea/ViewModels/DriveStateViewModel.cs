using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

public class DriveStateViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "Schedule")]
    public string Schedule { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(ShiftDurationTime))]
    public string ShiftDurationTime { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(CustomerLastAndFirstName))]
    public string CustomerLastAndFirstName { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(PickupDateAndTime))]
    public string PickupDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(City))]
    public string City { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(VehicleIdentifier))]
    public string VehicleIdentifier { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(Drive), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    [Display(ResourceType = typeof(Drive), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "DriveStatus")]
    public StatusOfDrive StatusOfDrive { get; set; }


    [Display(ResourceType = typeof(Drive), Name = "Comment")]
    public string CommentText { get; set; } = default!;

    [Display(ResourceType = typeof(Drive), Name = "AcceptedDateAndTime")]

    public string? DriveAcceptedDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "DeclineDateAndTime")]

    public string? DriveDeclineDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "InProgressDateAndTime")]

    public string? DriveInProgressDateAndTime { get; set; }

    [Display(ResourceType = typeof(Drive), Name = "FinishedDateAndTime")]
    public string? DriveFinishedDateAndTime { get; set; }

    public bool IsDriveAccepted { get; set; }

    public bool IsDriveDeclined { get; set; }

    public bool IsDriveStarted { get; set; }

    public bool IsDriveFinished { get; set; }
}