using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCountyViewModel
{
    public Guid Id { get; set; }
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("County Name")]
    public string CountyName { get; set; } = default!;
}