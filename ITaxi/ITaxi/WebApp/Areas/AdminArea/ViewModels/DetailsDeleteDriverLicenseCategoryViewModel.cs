using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDriverLicenseCategoryViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(DriverLicenseCategory), Name = "DriverLicenseCategoryName")]
    public string DriverLicenseCategoryName { get; set; } = default!;
}