using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class VehicleMapper : BaseMapper<App.BLL.DTO.AdminArea.VehicleDTO, App.DAL.DTO.AdminArea.VehicleDTO>
{
    public VehicleMapper(IMapper mapper) : base(mapper)
    {
    }
}