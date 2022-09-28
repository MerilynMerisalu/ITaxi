using Base.Domain;

namespace App.Domain.DTO;

public class CustomerDTO: DomainEntityMetaId
{
    public Guid DisabilityTypeId { get; set; }
}