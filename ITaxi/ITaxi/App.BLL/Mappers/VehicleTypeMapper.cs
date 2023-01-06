using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class VehicleTypeMapper : BaseMapper<App.BLL.DTO.AdminArea.VehicleTypeDTO, App.DAL.DTO.AdminArea.VehicleTypeDTO>
{
    public VehicleTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}