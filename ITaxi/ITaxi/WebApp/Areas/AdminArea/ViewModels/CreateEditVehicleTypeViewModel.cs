using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleTypeViewModel
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Type")]
    public string VehicleTypeName { get; set; } = default!;
}