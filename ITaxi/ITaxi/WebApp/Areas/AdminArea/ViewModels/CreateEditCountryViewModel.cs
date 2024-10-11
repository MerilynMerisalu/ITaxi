using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCountryViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
   
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MinLength(3, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [MaxLength(3, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Country), Name = nameof(CountryName))]
    public string CountryName { get; set; } = default!;


    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Country), Name = nameof(ISOCode))]
    [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [MinLength(3, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [MaxLength(3, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string ISOCode { get; set; } = default!;
}