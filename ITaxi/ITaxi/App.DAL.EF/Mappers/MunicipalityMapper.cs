using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MunicipalityMapper : BaseMapper<MunicipalityDTO, Domain.Municipality>
{
    public MunicipalityMapper(IMapper mapper) : base(mapper)
    {
    }
}