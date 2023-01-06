using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleMarkMapper: BaseMapper<App.DAL.DTO.AdminArea.VehicleMarkDTO, App.Domain.VehicleMark>
{
    public VehicleMarkMapper(IMapper mapper) : base(mapper)
    {
    }
}