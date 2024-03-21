using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit disability type view model
/// </summary>
public class CreateEditDisabilityTypeViewModel
{
    /// <summary>
    /// Disability type id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Disability type name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(80, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(DisabilityType), Name = nameof(DisabilityTypeName))]
    public string DisabilityTypeName { get; set; } = default!;
}