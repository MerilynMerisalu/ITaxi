using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class VehicleMarkMapper: BaseMapper<DTO.AdminArea.VehicleMarkDTO, App.DAL.DTO.AdminArea.VehicleMarkDTO>
{
    public VehicleMarkMapper(IMapper mapper) : base(mapper)
    {
    }
}