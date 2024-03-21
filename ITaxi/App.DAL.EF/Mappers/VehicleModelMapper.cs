using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleModelMapper : BaseMapper<DTO.AdminArea.VehicleModelDTO, Domain.VehicleModel>
{
    public VehicleModelMapper(IMapper mapper) : base(mapper)
    {
    }
}