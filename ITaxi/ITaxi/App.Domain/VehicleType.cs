using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class VehicleType: DomainEntityMetaId
{
    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Vehicle Type")]
    public string VehicleTypeName { get; set; } = default!;

    public ICollection<Vehicle>? Vehicles { get; set; }
}