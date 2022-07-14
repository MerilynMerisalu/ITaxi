using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleMarkViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(VehicleMark),  Name = nameof(VehicleMarkName))]
    public string VehicleMarkName { get; set; } = default!;
}