using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete driver license category view model
/// </summary>
public class DetailsDeleteDriverLicenseCategoryViewModel : AdminAreaBaseViewModel
{
    /// <summary>
    /// Driver license category id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Driver license category name
    /// </summary>
    [Display(ResourceType = typeof(DriverLicenseCategory), Name = "DriverLicenseCategoryName")]
    public string DriverLicenseCategoryName { get; set; } = default!;
}