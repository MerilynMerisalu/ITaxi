using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Http;

namespace App.Domain;

public class Photo: DomainEntityMetaId
{
    
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
   
}