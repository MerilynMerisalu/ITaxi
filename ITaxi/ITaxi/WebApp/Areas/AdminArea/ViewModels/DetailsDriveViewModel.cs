using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details drive view model
/// </summary>
public class DetailsDriveViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = "ScheduleName")]
    public string Schedule { get; set; } = default!;

    /// <summary>
    /// Shift duration time
    /// </summary>
    [Display(ResourceType = typeof(Schedule), Name = nameof(ShiftDurationTime))]
    public string ShiftDurationTime { get; set; } = default!;

    /// <summary>
    /// Driver last and first name
    /// </summary>
    [Display(ResourceType = typeof(Driver), Name = "JobTitle")]
    public string DriverLastAndFirstName { get; set; } = default!;

    /// <summary>
    /// Customer last and first name
    /// </summary>
    [Display(ResourceType = typeof(Customer), Name = "CustomerName")]
    public string CustomerLastAndFirstName { get; set; } = default!;

    /// <summary>
    /// Pick up date and time
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "PickUpDateAndTime")]
    public string PickupDateAndTime { get; set; } = default!;

    /// <summary>
    /// City
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(City))]
    public string City { get; set; } = default!;

    /// <summary>
    /// Pick up address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    /// <summary>
    /// Need assistance leaving the building
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceLeavingTheBuilding))]
    public bool NeedAssistanceLeavingTheBuilding { get; set; }

    /// <summary>
    /// Pickup floor number
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupFloorNumber))]
    public int PickupFloorNumber { get; set; }
    
    /// <summary>
    /// Destination address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    /// <summary>
    /// Needing assistance entering the building
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceEnteringTheBuilding))]
    public bool NeedAssistanceEnteringTheBuilding { get; set; }
    /// <summary>
    /// Destination floor number
    /// </summary>

    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationFloorNumber))]
    public int DestinationFloorNumber { get; set; }



    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// Vehicle identifier
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Vehicle")]
    public string VehicleIdentifier { get; set; } = default!;

    /// <summary>
    /// Number of passengers
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// Boolean has an assistant
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    /// <summary>
    /// Status of booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }

    /// <summary>
    /// Status of drive
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "StatusOfDrive")]
    public StatusOfDrive StatusOfDrive { get; set; }
    
    /// <summary>
    /// Comment text
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
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