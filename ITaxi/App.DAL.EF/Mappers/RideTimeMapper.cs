using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RideTimeMapper : BaseMapper<DTO.AdminArea.RideTimeDTO, Domain.RideTime>
{
    public RideTimeMapper(IMapper mapper) : base(mapper)
    {
    }
}