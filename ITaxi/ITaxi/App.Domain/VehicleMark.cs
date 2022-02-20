using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class VehicleMark: DomainEntityMetaId
{
    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Mark")]
    public string VehicleMarkName { get; set; } = default!;

    public ICollection<VehicleModel>? VehicleModels { get; set; }
}