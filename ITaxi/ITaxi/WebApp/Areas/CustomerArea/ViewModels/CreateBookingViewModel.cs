using System.ComponentModel.DataAnnotations;

using App.Resources.Areas.App.Domain.CustomerArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.CustomerArea.ViewModels;

/// <summary>
/// Create booking view model
/// </summary>
public class CreateBookingViewModel
{
    /// <summary>
    /// Create booking view model vehicle type id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    /// <summary>
    /// List of vehicle types
    /// </summary>
    public SelectList? VehicleTypes { get; set; }

    /// <summary>
    /// Create booking view model country id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "Country")]
    public Guid CountryId { get; set; }
    /// <summary>
    /// List of countries
    /// </summary>
    public SelectList? Countries { get; set; }

    /// <summary>
    /// County id
    /// </summary>

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "County")]
    public Guid CountyId { get; set; }
    /// <summary>
    /// List of counties
    /// </summary>
    public SelectList? Counties { get; set; }
    /// <summary>
    /// Municipality id
    /// </summary>

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "Municipality")]
    public Guid MunicipalityId { get; set; }
    /// <summary>
    /// List of municipalities
    /// </summary>
    public SelectList? Municipalities { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "locality")]
    public Guid LocalityId { get; set; }
    /// <summary>
    /// List of localities
    /// </summary>
    public SelectList? Localities{ get; set; }
    /// <summary>
    /// Pick up date and time
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public string PickUpDateAndTime { get; set; } = default!;

    /// <summary>
    /// Pickup address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string PickupAddress { get; set; } = default!;
    /// <summary>
    /// Need assistance leaving the building
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceLeavingTheBuilding))]
    public bool NeedAssistanceLeavingTheBuilding { get; set; }
    /// <summary>
    /// Pickup floor number
    /// </summary>
    [Range(minimum: 0, maximum: 35)]
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupFloorNumber))]
    public int PickupFloorNumber { get; set; }


    /// <summary>
    /// Does the pickup building have an elevator
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInThePickupBuilding))]
    public bool HasAnElevatorInThePickupBuilding { get; set; }


    /// <summary>
    /// Destination address
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string DestinationAddress { get; set; } = default!;

    /// <summary>
    /// Need assistance entering the building
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceEnteringTheBuilding))]
    public bool NeedAssistanceEnteringTheBuilding { get; set; }

    /// <summary>
    /// Destination floor number
    /// </summary>
    [Range(minimum: 0, maximum: 35)]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationFloorNumber))]
    public int DestinationFloorNumber { get; set; }

    /// <summary>
    /// Does the destination building have an elevator
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInTheDestinationBuilding))]
    public bool HasAnElevatorInTheDestinationBuilding { get; set; }

    /// <summary>
    /// Number of passengers
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int NumberOfPassengers { get; set; }

    /// <summary>
    /// Boolean has on assistant
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    [StringLength(1000, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Driver id
    /// </summary>
    public Guid DriverId { get; set; }
    
    /// <summary>
    /// Schedule id
    /// </summary>
    public Guid? ScheduleId { get; set; }
    
    /// <summary>
    /// Ride time id
    /// </summary>
    public Guid RideTimeId { get; set; }

    /// <summary>
    /// List of ride times
    /// </summary>
    public SelectList? RideTimes  { get; set; }

    /// <summary>
    /// Vehicle id
    /// </summary>
    public Guid VehicleId { get; set; }
}