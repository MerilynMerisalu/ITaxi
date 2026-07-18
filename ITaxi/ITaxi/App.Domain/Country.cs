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
        Name = nameof(ISOCodeAlpha2))]
    [MaxLength(2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(2, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    public string ISOCodeAlpha2 { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(ISOCodeAlpha3))]
    [MaxLength(3, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(3, MinimumLength = 3, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    public string ISOCodeAlpha3 { get; set; } = default!;

    public bool IsRegistrationSupported { get; set; }
    public ICollection<County>? Counties { get; set; }
    
    
}