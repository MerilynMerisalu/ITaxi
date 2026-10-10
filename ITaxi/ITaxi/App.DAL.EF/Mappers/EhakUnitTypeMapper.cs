using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class EhakUnitTypeMapper: BaseMapper<App.DAL.DTO.AdminArea.EhakUnitTypeDTO, App.Domain.EhakUnitType>
{
    public EhakUnitTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}