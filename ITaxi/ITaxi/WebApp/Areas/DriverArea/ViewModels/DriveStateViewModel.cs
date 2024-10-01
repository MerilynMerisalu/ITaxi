using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Drive state view model
/// </summary>
public class DriveStateViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "Schedule")]
    public string Schedule { get; set; } = default!;

    /// <summary>
    /// Shift duration time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(ShiftDurationTime))]
    public string ShiftDurationTime { get; set; } = default!;

    /// <summary>
    /// Customer last and first name
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "CustomerInfo")]
    public string CustomerFullNameAndPhoneNumber { get; set; } = default!;

    /// <summary>
    /// Pickup date and time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(PickupDateAndTime))]
    public string PickupDateAndTime { get; set; } = default!;

    /// <summary>
    /// City
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(City))]
    public string City { get; set; } = default!;

    /// <summary>
    /// Pickup address
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;
    /// <summary>
    /// Needing assistance leaving the building
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(NeedAssistanceLeavingTheBuilding) )]
    public bool NeedAssistanceLeavingTheBuilding { get; set; }

    /// <summary>
    /// Pick up floor number
    /// </summary>
    [Range(minimum: 0, maximum: 35)]
    [Display(ResourceType = typeof(Drive), Name = nameof(PickupFloorNumber))]
    public int PickupFloorNumber { get; set; }
    /// <summary>
    /// Destination address
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    /// <summary>
    /// Needing assistance entering the building
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(NeedAssistanceEnteringTheBuilding) )]
    public bool NeedAssistanceEnteringTheBuilding { get; set; }
    /// <summary>
    /// Destination floor number
    /// </summary>

    [Display(ResourceType = typeof(Drive), Name = nameof(DestinationFloorNumber))]
    public int DestinationFloorNumber { get; set; }

    /// <summary>
    /// Does the destination building have an elevator
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(HasAnElevatorInTheDestinationBuilding))]
    public bool HasAnElevatorInTheDestinationBuilding { get; set; }

    /// <summary>
    /// Does the pickup building have an elevator
    /// </summary>

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Drive), Name = nameof(HasAnElevatorInThePickupBuilding))]
    public bool HasAnElevatorInThePickupBuilding { get; set; }


    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// Vehicle identifier
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(VehicleIdentifier))]
    public string VehicleIdentifier { get; set; } = default!;

    /// <summary>
    /// Number of passengers
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// Has an assistant
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    /// <summary>
    /// Status of booking
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }

    /// <summary>
    /// Status of drive
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "DriveStatus")]
    public StatusOfDrive StatusOfDrive { get; set; }

    /// <summary>
    /// Comment text
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "Comment")]
    public string CommentText { get; set; } = default!;

    /// <summary>
    /// Drive accepted date and time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "AcceptedDateAndTime")]
    public string? DriveAcceptedDateAndTime { get; set; }

    /// <summary>
    /// Drive decline date and time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "DeclineDateAndTime")]
    public string? DriveDeclineDateAndTime { get; set; }

    /// <summary>
    /// Drive in progress date and time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "InProgressDateAndTime")]
    public string? DriveInProgressDateAndTime { get; set; }

    /// <summary>
    /// Drive finished date and time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "FinishedDateAndTime")]
    public string? DriveFinishedDateAndTime { get; set; }

    /// <summary>
    /// Boolean is drive accepted
    /// </summary>
    public bool IsDriveAccepted { get; set; }

    /// <summary>
    /// Boolean is drive declined
    /// </summary>
    public bool IsDriveDeclined { get; set; }

    /// <summary>
    /// Boolean is drive started
    /// </summary>
    public bool IsDriveStarted { get; set; }

    /// <summary>
    /// Boolean is drive finished
    /// </summary>
    public bool IsDriveFinished { get; set; }
}