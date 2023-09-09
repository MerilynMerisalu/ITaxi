using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CountryMapper : BaseMapper<CountryDTO,Domain.Country>
{
    public CountryMapper(IMapper mapper) : base(mapper)
    {
    }
}