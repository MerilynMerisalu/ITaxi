using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditDisabilityTypeViewModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(80)]
    [DisplayName("Disability Type")]
    public string DisabilityTypeName { get; set; } = default!;
}