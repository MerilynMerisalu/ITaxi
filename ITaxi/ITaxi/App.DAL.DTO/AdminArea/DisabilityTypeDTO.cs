using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class DisabilityTypeDTO: DomainEntityMetaId
{
    [MaxLength(80, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.DisabilityType),
        Name = nameof(DisabilityTypeName))]
    public LangStr DisabilityTypeName { get; set; } = default!;
}