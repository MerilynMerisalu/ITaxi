using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace ITaxi.Public.DTO.v1.AdminArea;

public class VehicleMark
{
    public Guid Id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleMark), Name = "VehicleMarkName")]
    public string VehicleMarkName { get; set; } = default!;

   
}