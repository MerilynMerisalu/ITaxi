using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Public.App.DTO.v1;

namespace Public.App.DTO.v1.AdminArea;

public class VehicleMark : Entity
{

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.VehicleMark), Name = "VehicleMarkName")]
    public string VehicleMarkName { get; set; } = default!;

    public override string ToString()
    {
        return VehicleMarkName;
    }
}