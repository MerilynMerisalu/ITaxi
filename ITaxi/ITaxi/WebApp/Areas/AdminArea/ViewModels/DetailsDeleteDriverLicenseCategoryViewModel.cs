using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDriverLicenseCategoryViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.DriverLicenseCategory), Name = "DriverLicenseCategoryName")]
    public string DriverLicenseCategoryName { get; set; } = default!;
}