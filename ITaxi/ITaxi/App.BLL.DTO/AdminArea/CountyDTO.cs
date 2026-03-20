using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class CountyDTO : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.County)
        , Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;
    public Guid CountryId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.County),
        Name = nameof(Country))]
    public CountryDTO? Country { get; set; }
    
    public int NumberOfCities { get; set; }

    [MaxLength(4, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.County),
        Name = nameof(CountyEHAKCode))]
    public string? CountyEHAKCode { get; set; }
}