using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Identity;

public class RefreshToken: DomainEntityId
{
    [StringLength(36, MinimumLength = 36)]
    public string Token { get; set; } = Guid.NewGuid().ToString();

    /* UTC */
    public DateTime TokenExpirationDateAndTime { get; set; } = DateTime.Now.AddDays(7);
    
    [StringLength(36, MinimumLength = 36)] 
    public string? PreviousToken { get; set; } 

    /* UTC */
    public DateTime? PreviousTokenExpirationDateAndTime { get; set; }
}