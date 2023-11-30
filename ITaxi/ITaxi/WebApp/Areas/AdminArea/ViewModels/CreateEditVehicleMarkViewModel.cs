using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit vehicle mark view model
/// </summary>
public class CreateEditVehicleMarkViewModel
{
    /// <summary>
    /// Vehicle mark id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vehicle mark name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(VehicleMark), Name = nameof(VehicleMarkName))]
    public string VehicleMarkName { get; set; } = default!;
}