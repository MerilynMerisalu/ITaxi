using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Http;

namespace App.Domain;

public class Photo: DomainEntityMetaId
{

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    public string Title { get; set; } = default!;
    
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(Name = "Photo Name")]
    public string? PhotoName { get; set; } 
    
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    [NotMapped] 
    public IFormFile? ImageFile { get; set; }
    
   
    
    
}