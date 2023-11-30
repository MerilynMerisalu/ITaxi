using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace ITaxi.Public.DTO.v1.AdminArea;

public class VehicleType 
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleType), Name = "VehicleTypeName")]
    public string VehicleTypeName { get; set; } = default!;

    public override string ToString()
    {
        return this.VehicleTypeName;
    }


}