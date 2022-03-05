using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleModelViewModel
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Model")]
    public string VehicleModelName { get; set; } = default!;

    [DisplayName("Vehicle Mark")]
    public Guid VehicleMarkId { get; set; }

    public SelectList? VehicleMarks { get; set; }

}