using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CityMapper: BaseMapper<App.BLL.DTO.AdminArea.CityDTO, App.DAL.DTO.AdminArea.CityDTO>
{
    public CityMapper(IMapper mapper) : base(mapper)
    {
    }
}