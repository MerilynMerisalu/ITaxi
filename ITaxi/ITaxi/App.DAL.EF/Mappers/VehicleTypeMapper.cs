using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleTypeMapper : BaseMapper<VehicleTypeDTO, Domain.VehicleType>
{
    public VehicleTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}