using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
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
    /// Create booking view model city id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "City")]
    public Guid CityId { get; set; }

    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }

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
    /// Destination address
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string DestinationAddress { get; set; } = default!;

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