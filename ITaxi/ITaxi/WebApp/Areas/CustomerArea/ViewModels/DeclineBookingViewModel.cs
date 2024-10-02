using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.CustomerArea;

namespace WebApp.Areas.CustomerArea.ViewModels;

/// <summary>
/// Decline booking view model
/// </summary>
public class DeclineBookingViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle Information
    /// </summary>

    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleInfo))]
    public string? VehicleInfo { get; set; }

    /// <summary>
    /// Driver Information
    /// </summary>

    [Display(ResourceType = typeof(Booking), Name = nameof(DriverInfo))]
    public string? DriverInfo { get; set; }

    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// City
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(City))]
    public string City { get; set; } = default!;

    /// <summary>
    /// Pick up date and time
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]
    public string PickUpDateAndTime { get; set; } = default!;

    /// <summary>
    /// Pick up address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    /// <summary>
    /// Destination address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

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
    /// Does the pickup building have an elevator?
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInThePickupBuilding))]
    public bool HasAnElevatorInThePickupBuilding { get; set; }

    /// <summary>
    /// Does the destination building have an elevator?
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInTheDestinationBuilding))]
    public bool HasAnElevatorInTheDestinationBuilding { get; set; }

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
    /// Number of passengers
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// Has an assistant
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    /// <summary>
    /// Booking additional info
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Status of booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(StatusOfBooking))]
    public StatusOfBooking StatusOfBooking { get; set; }
}