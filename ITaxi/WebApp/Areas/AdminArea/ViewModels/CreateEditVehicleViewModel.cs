using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Enum.Enum;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle = App.Resources.Areas.App.Domain.AdminArea.Vehicle;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit vehicle view model
/// </summary>
public class CreateEditVehicleViewModel
{
    /// <summary>
    /// Vehicle id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle type id 
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    /// <summary>
    /// Vehicle mark id 
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleMark")]
    public Guid VehicleMarkId { get; set; }

    /// <summary>
    /// Vehicle model id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleModel")]
    public Guid VehicleModelId { get; set; }

    /// <summary>
    /// List of vehicle types
    /// </summary>
    public SelectList? VehicleTypes { get; set; }
    
    /// <summary>
    /// List of vehicle marks
    /// </summary>
    public SelectList? VehicleMarks { get; set; }
    
    /// <summary>
    /// List of vehicle models
    /// </summary>
    public SelectList? VehicleModels { get; set; }

    /// <summary>
    /// Vehicle plate number
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehiclePlateNumber")]
    public string VehiclePlateNumber { get; set; } = default!;

    /// <summary>
    /// Vehicle manufacture year
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "ManufactureYear")]
    public int ManufactureYear { get; set; }

    /// <summary>
    /// List of manufacture years
    /// </summary>
    public SelectList? ManufactureYears { get; set; }

    /// <summary>
    /// Number of seats
    /// </summary>
    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Vehicle), Name = nameof(NumberOfSeats))]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }

    /// <summary>
    /// Vehicle availability
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }

    /// <summary>
    /// Driver id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "Driver")]
    public Guid DriverId { get; set; }

    /// <summary>
    /// Driver
    /// </summary>
    public Driver? Driver { get; set; }
    
    /// <summary>
    /// List of drivers
    /// </summary>
    public SelectList? Drivers { get; set; }
}