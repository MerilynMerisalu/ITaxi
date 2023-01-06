using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class VehicleMarkDTO:DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleMark), Name = "VehicleMarkName")]
    public string VehicleMarkName { get; set; } = default!;

    public ICollection<VehicleModelDTO>? VehicleModels { get; set; }
}