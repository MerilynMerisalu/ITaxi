using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCountyViewModel
{
    public Guid Id { get; set; }
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Base.Resources.Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County),
        Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;

    
}