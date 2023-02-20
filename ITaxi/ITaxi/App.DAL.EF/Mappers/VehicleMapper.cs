using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleMapper : BaseMapper<DTO.AdminArea.VehicleDTO, Domain.Vehicle>
{
    public VehicleMapper(IMapper mapper) : base(mapper)
    {
    }
}