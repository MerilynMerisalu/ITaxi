using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Photo: DomainEntityMetaId
{
    [Required]
    [MaxLength(150)]
    [StringLength(150)]
    public string Title { get; set; } = default!;
    
    [Required]
    [MaxLength(150)]
    [StringLength(150)]
    public string PhotoURL { get; set; } = default!;

    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public byte[]? ProfilePicture { get; set; }
    
}