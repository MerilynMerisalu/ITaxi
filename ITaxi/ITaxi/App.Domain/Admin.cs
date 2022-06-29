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
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "City")] 
    public Guid CityId { get; set; }

    public City? City { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "AddressOfResidence")]
    public string Address { get; set; } = default!;
}