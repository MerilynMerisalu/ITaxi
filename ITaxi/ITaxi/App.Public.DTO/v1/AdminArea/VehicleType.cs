using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.AdminArea;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea;

public class VehicleType : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleType), Name = "VehicleTypeName")]
    public string VehicleTypeName { get; set; } = default!;


   // public ICollection<Vehicle>? Vehicles { get; set; }

}