using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class VehicleModelDTO : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string VehicleModelName { get; set; } = default!;

    public Guid VehicleMarkId { get; set; }

    public VehicleMarkDTO? VehicleMark { get; set; }
    
}