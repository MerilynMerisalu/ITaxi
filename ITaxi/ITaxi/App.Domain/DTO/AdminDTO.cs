using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.DTO;

public class AdminDTO: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(50)]
    [StringLength(50)]
    public string? PersonalIdentifier { get; set; }

    
    public Guid CityId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = "City")]

    public City? City { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    
    public string Address { get; set; } = default!;
}