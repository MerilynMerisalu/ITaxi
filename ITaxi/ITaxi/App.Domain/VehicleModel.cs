using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class VehicleModel: DomainEntityMetaId
{

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Model")]
    public string VehicleModelName { get; set; } = default!;

    [DisplayName("Vehicle Mark")] public Guid VehicleMarkId { get; set; }

    [DisplayName("Vehicle Mark")] public VehicleMark? VehicleMark { get; set; }
}