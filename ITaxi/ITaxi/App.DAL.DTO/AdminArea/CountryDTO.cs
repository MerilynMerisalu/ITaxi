using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class CountryDTO : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "ErrorMessageMaxLength")]
    
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



}