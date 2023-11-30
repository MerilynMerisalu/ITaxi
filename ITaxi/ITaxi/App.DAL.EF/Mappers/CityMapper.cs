using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CityMapper : BaseMapper<CityDTO, Domain.City>
{
    public CityMapper(IMapper mapper) : base(mapper)
    {
    }
}