using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDriverLicenseCategoryViewModel
{
    public Guid Id { get; set; }
    [DisplayName("Driver License Category Name")]
    public string DriverLicenseCategoryName { get; set; } = default!;
}