using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;
using Base.Resources;

namespace App.DTO.AdminArea;

public class CityDTO: DomainEntityId
{
    public Guid CountyId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.City),
        Name = "CountyName")]
    public County? County { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.City),
        Name = nameof(CityName))]
    public string CityName { get; set; } = default!;
}