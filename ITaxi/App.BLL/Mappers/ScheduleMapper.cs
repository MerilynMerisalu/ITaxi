using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ScheduleMapper: BaseMapper<DTO.AdminArea.ScheduleDTO, App.DAL.DTO.AdminArea.ScheduleDTO>
{
    public ScheduleMapper(IMapper mapper) : base(mapper)
    {
    }
}