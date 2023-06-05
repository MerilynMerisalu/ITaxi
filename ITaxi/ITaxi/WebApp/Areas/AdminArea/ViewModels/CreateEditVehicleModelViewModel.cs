using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit vehicle model view model
/// </summary>
public class CreateEditVehicleModelViewModel
{
    /// <summary>
    /// Vehicle model id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle model name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    /// <summary>
    /// Vehicle mark id 
    /// </summary>
    [Display(ResourceType = typeof(VehicleMark), Name = "VehicleMarkName")]
    public Guid VehicleMarkId { get; set; }

    /// <summary>
    /// List of vehicle marks
    /// </summary>
    public SelectList? VehicleMarks { get; set; }
}