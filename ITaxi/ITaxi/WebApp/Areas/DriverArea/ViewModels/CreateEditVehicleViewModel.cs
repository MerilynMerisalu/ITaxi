using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.DriverArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class CreateEditVehicleViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Vehicle), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    [Display(ResourceType = typeof(Vehicle), Name = "VehicleMark")]
    public Guid VehicleMarkId { get; set; }

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

    [Display(ResourceType = typeof(Vehicle), Name = "ManufactureYear")]
    public int ManufactureYear { get; set; }


    public SelectList? ManufactureYears { get; set; }

    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Vehicle), Name = nameof(NumberOfSeats))]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }

    [Display(ResourceType = typeof(Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }
}