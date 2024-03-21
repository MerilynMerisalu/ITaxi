using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class CityDTO: DomainEntityMetaId
{
    public Guid CountyId { get; set; }

    //[Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.City),
        //Name = "CountyName")]
    public CountyDTO? County { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    //[Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.City),
       // Name = nameof(CityName))]
    public string CityName { get; set; } = default!;
}