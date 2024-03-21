using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CountryMapper: BaseMapper<DTO.AdminArea.CountryDTO,
    App.DAL.DTO.AdminArea.CountryDTO>
{
    public CountryMapper(IMapper mapper) : base(mapper)
    {
    }
}