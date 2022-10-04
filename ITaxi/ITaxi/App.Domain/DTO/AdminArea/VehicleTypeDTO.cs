using Base.Domain;

namespace App.Domain.DTO.AdminArea;

public class VehicleTypeDTO: DomainEntityMetaId
{
    public string VehicleTypeName { get; set; } = default!;
}