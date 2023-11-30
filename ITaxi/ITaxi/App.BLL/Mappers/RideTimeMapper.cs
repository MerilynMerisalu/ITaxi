using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class RideTimeMapper : BaseMapper<DTO.AdminArea.RideTimeDTO, App.DAL.DTO.AdminArea.RideTimeDTO>
{
    public RideTimeMapper(IMapper mapper) : base(mapper)
    {
    }
}