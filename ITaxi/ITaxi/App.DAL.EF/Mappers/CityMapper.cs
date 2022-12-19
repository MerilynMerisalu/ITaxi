using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CityMapper: BaseMapper<App.DTO.AdminArea.CityDTO, App.Domain.City>
{
    public CityMapper(IMapper mapper) : base(mapper)
    {
    }
}