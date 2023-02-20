using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DisabilityTypeMapper: BaseMapper<DTO.AdminArea.DisabilityTypeDTO, App.Domain.DisabilityType>
{
    public DisabilityTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}