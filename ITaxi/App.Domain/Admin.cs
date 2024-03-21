using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Admin : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    public string? PersonalIdentifier { get; set; }
    
    public Guid CityId { get; set; }

    public City? City { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1)]
    
    public string Address { get; set; } = default!;
}