using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit county view model
/// </summary>
public class CreateEditCountyViewModel
{
    /// <summary>
    /// County id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// County name
    /// </summary>
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(County),
        Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;
}