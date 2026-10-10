using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class EhakUnitTypeDTO: DomainEntityMetaId
{
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.EhakUnitType), Name = "EhakUnitTypeName")]
    public string EhakUnitTypeName { get; set; } = default!;
        
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MinLength(6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
    [MaxLength(6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.EhakUnitType), 
        Name = "EhakUnitTypeCode")]
    public string EhakUnitTypeCode { get; set; } = default!;
}