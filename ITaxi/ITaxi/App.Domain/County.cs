using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Base.Domain;

namespace App.Domain;

public class County: DomainEntityMetaId
{
    
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County)
        , Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;

    public ICollection<City>? Cities { get; set; }
}