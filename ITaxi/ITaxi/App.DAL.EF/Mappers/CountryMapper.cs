using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CountryMapper : BaseMapper<CountryDTO,App.Domain.Country>
{
    public CountryMapper(IMapper mapper) : base(mapper)
    {
    }
}