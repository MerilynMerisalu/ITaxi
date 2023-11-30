using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class VehicleModel : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string VehicleModelName { get; set; } = default!;
    
    public Guid VehicleMarkId { get; set; }

    public VehicleMark? VehicleMark { get; set; }
}