using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RideTimeMapper : BaseMapper<App.DAL.DTO.AdminArea.RideTimeDTO, App.Domain.RideTime>
{
    public RideTimeMapper(IMapper mapper) : base(mapper)
    {
    }
}