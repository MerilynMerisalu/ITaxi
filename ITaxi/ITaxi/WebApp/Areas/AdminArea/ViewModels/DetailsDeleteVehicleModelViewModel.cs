using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using VehicleMark = App.Resources.Areas.App.Domain.AdminArea.VehicleMark;
using VehicleModel = App.Resources.Areas.App.Domain.AdminArea.VehicleModel;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleModelViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    public Guid VehicleMarkId { get; set; }

    [Display(ResourceType = typeof(VehicleMark), Name = nameof(VehicleMarkName))] 
    public string VehicleMarkName { get; set; } = default!;
}