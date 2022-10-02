using Base.Domain;

namespace App.Domain.DTO;

public class VehicleTypeDTO: DomainEntityMetaId
{
    public string VehicleTypeName { get; set; }
}