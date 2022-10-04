using Base.Domain;

namespace App.Domain.DTO.AdminArea;

public class DisabilityTypeDTO: DomainEntityMetaId
{ 
    public string DisabilityTypeName { get; set; } = default!;
}