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
    public LangStr CountryName { get; set; } = default!;
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), 
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(ISOCode))]
    public string ISOCode { get; set; } = default!;

}