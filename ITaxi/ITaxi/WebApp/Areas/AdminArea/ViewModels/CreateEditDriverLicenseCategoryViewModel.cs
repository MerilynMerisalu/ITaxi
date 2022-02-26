using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditDriverLicenseCategoryViewModel
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Driver License Category Name")]
    public string DriverLicenseCategoryName { get; set; } = default!;

}