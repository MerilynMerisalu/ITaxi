using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit driver license category view model
/// </summary>
public class CreateEditDriverLicenseCategoryViewModel
{
    /// <summary>
    /// Driver license category id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver license category name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(DriverLicenseCategory), Name = "DriverLicenseCategoryName")]
    public string DriverLicenseCategoryName { get; set; } = default!;
}