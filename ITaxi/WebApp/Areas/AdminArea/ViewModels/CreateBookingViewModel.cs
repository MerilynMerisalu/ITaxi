using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create booking view model
/// </summary>
public class CreateBookingViewModel
{
    /// <summary>
    /// Schedule id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Schedule")]
    public Guid? ScheduleId { get; set; }

    /// <summary>
    /// Driver id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Driver")]
    public Guid DriverId { get; set; }

    /// <summary>
    /// Customer id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Customer")]
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Vehicle id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "Vehicle")]
    public Guid VehicleId { get; set; }

    /// <summary>
    /// Vehicle type id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    /// <summary>
    /// Vehicle type for create booking
    /// </summary>
    public SelectList? VehicleTypes { get; set; }

    /// <summary>
    /// City id for create booking
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = "City")]
    public Guid CityId { get; set; }

    /// <summary>
    /// List of cities to create booking
    /// </summary>
    public SelectList? Cities { get; set; }

    /// <summary>
    /// Pick up date and time
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = false)]
    public string PickUpDateAndTime { get; set; } = default!;

    /// <summary>
    /// Pick up address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    [StringLength(50, MinimumLength = 1)]
    public string PickupAddress { get; set; } = default!;

    /// <summary>
    /// Destination address
    /// </summary>
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string DestinationAddress { get; set; } = default!;

    /// <summary>
    /// Number of passengers
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
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
    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// List of schedules
    /// </summary>
    public SelectList? Schedules { get; set; }

    /// <summary>
    /// List of drivers
    /// </summary>
    public SelectList? Drivers { get; set; }

    /// <summary>
    /// List of customers
    /// </summary>
    public SelectList? Customers { get; set; }
    
    /// <summary>
    /// List of vehicles
    /// </summary>
    public SelectList? Vehicles { get; set; }
    
    /// <summary>
    /// List of available RideTimes for the user to select from
    /// </summary>
    public SelectList? RideTimes { get; set; }

    /// <summary>
    /// Selected Ride Time
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public Guid RideTimeId { get; set; }
}