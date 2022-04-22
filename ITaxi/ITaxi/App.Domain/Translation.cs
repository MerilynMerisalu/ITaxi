using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Translation: DomainEntityId
{
    [MaxLength(5)]
    public string Culture { get; set; } = default!;
    [MaxLength(5)]
    public string Value { get; set; } = default!;
    
    #warning Continue with translations after you already have a DTO layer

}