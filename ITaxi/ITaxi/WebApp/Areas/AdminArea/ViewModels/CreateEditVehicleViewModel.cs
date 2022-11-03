using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Enum;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle = App.Resources.Areas.App.Domain.AdminArea.Vehicle;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleMark")]
    public Guid VehicleMarkId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleModel")]
    public Guid VehicleModelId { get; set; }

    public SelectList? VehicleTypes { get; set; }
    public SelectList? VehicleMarks { get; set; }
    public SelectList? VehicleModels { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehiclePlateNumber")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "ManufactureYear")]
    public int ManufactureYear { get; set; }


    public SelectList? ManufactureYears { get; set; }

    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Vehicle), Name = nameof(NumberOfSeats))]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Vehicle), Name = "Driver")]
    public Guid DriverId { get; set; }

    public Driver? Driver { get; set; }


    public SelectList? Drivers { get; set; }
}