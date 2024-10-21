using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Country: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), 
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MaxLength(50, ErrorMessageResourceType =typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMax))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(CountryName))]
    public LangStr CountryName { get; set; } = default!;
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), 
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(ISOCode))]
    public string ISOCode { get; set; } = default!;

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(IsIgnored))]
    public bool IsIgnored { get; set; }
    
    public ICollection<County>? Counties { get; set; }
    
    
}