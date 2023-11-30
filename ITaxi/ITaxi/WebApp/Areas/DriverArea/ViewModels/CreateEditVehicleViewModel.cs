using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using App.Resources.Areas.App.Domain.DriverArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Create edit vehicle view model
/// </summary>
public class CreateEditVehicleViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle type id
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    /// <summary>
    /// Vehicle mark id
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleMark")]
    public Guid VehicleMarkId { get; set; }

    /// <summary>
    /// Vehicle model id
    /// </summary>
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleModel")]
    public Guid VehicleModelId { get; set; }

    /// <summary>
    /// List of vehisle types
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
    [Display(ResourceType = typeof(Vehicle), Name = "ManufactureYear")]
    public int ManufactureYear { get; set; }
    
    /// <summary>
    /// List of years
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
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }
}