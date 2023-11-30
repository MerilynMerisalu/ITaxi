using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class VehicleModelMapper : BaseMapper<DTO.AdminArea.VehicleModelDTO, App.DAL.DTO.AdminArea.VehicleModelDTO>
{
    public VehicleModelMapper(IMapper mapper) : base(mapper)
    {
    }
}