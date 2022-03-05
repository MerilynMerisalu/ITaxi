using System.ComponentModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleTypeViewModel
{
    public Guid Id { get; set; }
    [DisplayName("Vehicle Type")]
    public string VehicleTypeName { get; set; } = default!;
}