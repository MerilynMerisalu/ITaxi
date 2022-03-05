using System.ComponentModel;
using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleModelViewModel
{
    public Guid Id { get; set; }
    [DisplayName("Vehicle Model")]
    public string VehicleModelName { get; set; } = default!;

    public Guid VehicleMarkId { get; set; }

    [DisplayName("Vehicle Mark")] public string VehicleMarkName { get; set; } = default!;
}