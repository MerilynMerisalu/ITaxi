using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Admin : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public string? PersonalIdentifier { get; set; }
    
}