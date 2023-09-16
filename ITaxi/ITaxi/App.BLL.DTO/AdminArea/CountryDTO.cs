using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class CountryDTO: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Country)
        , Name = nameof(CountryName))]
    public string CountryName { get; set; } = default!;

}