using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit vehicle type view model
/// </summary>
public class CreateEditVehicleTypeViewModel
{
    /// <summary>
    /// Vehicle type id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle type name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(VehicleType), Name = "VehicleTypeName")]
    public string VehicleTypeName { get; set; } = default!;
}