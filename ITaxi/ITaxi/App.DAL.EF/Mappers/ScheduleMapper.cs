using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ScheduleMapper: BaseMapper<App.DAL.DTO.AdminArea.ScheduleDTO, App.Domain.Schedule>
{
    public ScheduleMapper(IMapper mapper) : base(mapper)
    {
    }
}