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
    
    public string CountryName { get; set; } = default!;

}