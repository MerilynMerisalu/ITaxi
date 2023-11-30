using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea;

public class Admin: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string? PersonalIdentifier { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = "City")]
    public Guid CityId { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; } = default!;
    

}