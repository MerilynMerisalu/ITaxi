using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea;

public class DisabilityType : DomainEntityMetaId
{
    [MaxLength(80, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.DisabilityType),
        Name = nameof(DisabilityTypeName))]
    public string DisabilityTypeName { get; set; } = default!;
}