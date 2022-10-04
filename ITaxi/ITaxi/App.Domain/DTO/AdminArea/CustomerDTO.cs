using Base.Domain;

namespace App.Domain.DTO.AdminArea;

public class CustomerDTO: DomainEntityMetaId
{
    public Guid DisabilityTypeId { get; set; }
}