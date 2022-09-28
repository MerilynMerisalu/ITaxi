using Base.Domain;

namespace App.Domain.DTO;

public class DisabilityTypeDTO: DomainEntityMetaId
{
    
    public string DisabilityTypeName { get; set; } = default!;

}