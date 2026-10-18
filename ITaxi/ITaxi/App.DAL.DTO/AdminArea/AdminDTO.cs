using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class AdminDTO: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    public string? PersonalIdentifier { get; set; }

    
    public Guid LocalityId { get; set; }

    public LocalityDTO? Locality { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1)]
    
    public string Address { get; set; } = default!;
}