using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;
public class DetailsDeleteVehicleTypeViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    [Display(ResourceType = typeof(VehicleType), Name = nameof(VehicleTypeName))]
    public string VehicleTypeName { get; set; } = default!;
}