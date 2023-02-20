using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ScheduleMapper: BaseMapper<DTO.AdminArea.ScheduleDTO, Domain.Schedule>
{
    public ScheduleMapper(IMapper mapper) : base(mapper)
    {
    }
}