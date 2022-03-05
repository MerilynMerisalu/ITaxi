using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleMarkViewModel
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Mark")]
    public string VehicleMarkName { get; set; } = default!;
}