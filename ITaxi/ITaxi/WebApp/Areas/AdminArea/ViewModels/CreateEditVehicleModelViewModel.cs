using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleModelViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    [Display(ResourceType = typeof(VehicleMark), Name = "VehicleMarkName")]
    public Guid VehicleMarkId { get; set; }

    public SelectList? VehicleMarks { get; set; }

}