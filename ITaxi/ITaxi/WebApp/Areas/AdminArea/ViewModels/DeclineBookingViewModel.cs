using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Decline booking view model
/// </summary>
public class DeclineBookingViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Booking id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Shift duration time
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Schedule")]
    public string ShiftDurationTime { get; set; } = default!;

    /// <summary>
    /// Customer
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(Customer))]
    public string Customer { get; set; } = default!;
    
    /// <summary>
    /// Driver
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(Driver))]
    public string Driver { get; set; } = default!;

    /// <summary>
    /// Vehicle type
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    /// <summary>
    /// Vehicle
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(Vehicle))]
    public string Vehicle { get; set; } = default!;

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
    /// Needing assistance leaving the building
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceLeavingTheBuilding))]
    public bool NeedAssistanceLeavingTheBuilding { get; set; }

    /// <summary>
    /// Pick up floor number
    /// </summary>
    [Range(minimum: 0, maximum: 35)]
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