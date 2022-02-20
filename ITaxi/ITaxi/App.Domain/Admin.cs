using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Admin: DomainEntityMetaId
{
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(50)]
    [StringLength(50)]
    [DisplayName("Personal Identifier")]
    public string? PersonalIdentifier { get; set; }

    [DisplayName("City")] public Guid CityId { get; set; }

    public City? City { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; } = default!;
}