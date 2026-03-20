using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class County : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string CountyName { get; set; } = default!;
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string CountyNameNormalized { get; set; } = default!;
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }
    
    [MaxLength(4, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.County), Name = nameof(CountyEHAKCode) )]
    public string? CountyEHAKCode { get; set; }

    public ICollection<City>? Cities { get; set; }
}