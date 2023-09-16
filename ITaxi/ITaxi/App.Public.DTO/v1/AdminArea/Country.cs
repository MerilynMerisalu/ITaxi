using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea;

public class Country: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(CountryName))]
    
    public string CountryName { get; set; } = default!;
}