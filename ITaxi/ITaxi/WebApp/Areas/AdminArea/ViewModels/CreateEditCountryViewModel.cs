using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCountryViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    [Display(ResourceType = typeof(Country), Name = nameof(CountryName))]
    public string CountryName { get; set; } = default!;


    [MaxLength(2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(2, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    public string ISOCodeAlpha2 { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Country),
        Name = nameof(ISOCodeAlpha3))]
    [MaxLength(2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
    [StringLength(2, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    public string ISOCodeAlpha3 { get; set; } = default!;
}
