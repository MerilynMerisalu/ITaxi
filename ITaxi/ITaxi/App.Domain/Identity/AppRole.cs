using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;
using Base.Domain;

namespace App.Domain.Identity;

public class AppRole : BaseRole
{
    [MinLength(1)] 
    [MaxLength(128)] 
    public LangStr DisplayName { get; set; } = default!;
}