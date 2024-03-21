using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleMarkMapper: BaseMapper<DTO.AdminArea.VehicleMarkDTO, Domain.VehicleMark>
{
    public VehicleMarkMapper(IMapper mapper) : base(mapper)
    {
    }
}