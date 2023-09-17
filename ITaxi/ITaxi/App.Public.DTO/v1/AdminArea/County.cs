using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea;

public class County : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public Guid CountryId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County), Name = "Country")]
    public Country? Country { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    
    public string CountyName { get; set; } = default!;

    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County), Name = "NumberOfCities")]
    public int NumberOfCities { get; set; }
}