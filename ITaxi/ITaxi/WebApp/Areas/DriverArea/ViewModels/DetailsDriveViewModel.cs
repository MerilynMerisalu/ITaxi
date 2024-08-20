using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.DriverArea;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Details drive view model
/// </summary>
public class DetailsDriveViewModel
{
    /// <summary>
    /// Details drive view model id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(Schedule))]
    public string Schedule { get; set; } = default!;

    /// <summary>
    /// Shift duration time
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(ShiftDurationTime))]
    public string ShiftDurationTime { get; set; } = default!;

    /// <summary>
    /// Customer last and first name
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "CustomerLastAndFirstName")]
    public string CustomerLastAndFirstName { get; set; } = default!;

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
    /// Pick up address
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    /// <summary>
    /// Destination address
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// Vehicle identifier
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = "VehicleIdentifier")]
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
    /// Rating for the drive
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Comment), Name = "Rating")]
    [Range(minimum:1, maximum:5)]
    public int? StarRating { get; set; }
    
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
    /// Is drive accepted
    /// </summary>
    public bool IsDriveAccepted { get; set; }

    /// <summary>
    /// Is drive declined
    /// </summary>
    public bool IsDriveDeclined { get; set; }

    /// <summary>
    /// Is drive started
    /// </summary>
    public bool IsDriveStarted { get; set; }

    /// <summary>
    /// Is drive finished
    /// </summary>
    public bool IsDriveFinished { get; set; }

    /// <summary>
    /// Status of booking
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }

    /// <summary>
    /// Status of drive
    /// </summary>
    [Display(ResourceType = typeof(Drive), Name = nameof(StatusOfDrive))]
    public StatusOfDrive StatusOfDrive { get; set; }
}