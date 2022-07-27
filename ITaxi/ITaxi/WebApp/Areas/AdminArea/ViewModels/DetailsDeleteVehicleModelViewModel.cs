using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleModelViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    public Guid VehicleMarkId { get; set; }

    [Display(ResourceType = typeof(VehicleMark), Name = nameof(VehicleMarkName))]
    public string VehicleMarkName { get; set; } = default!;
}