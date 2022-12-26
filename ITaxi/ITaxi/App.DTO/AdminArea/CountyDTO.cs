using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;
using Base.Resources;

namespace App.DTO.AdminArea;

public class CountyDTO: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.County)
        , Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;

    //public ICollection<City>? Cities { get; set; }
    public int NumberOfCities { get; set; }
}