using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class County : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.County)
        , Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;

    public ICollection<City>? Cities { get; set; }
}