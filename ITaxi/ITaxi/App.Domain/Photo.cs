using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Photo: DomainEntityMetaId
{
    

    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public byte[]? ProfilePhoto { get; set; }
    
}