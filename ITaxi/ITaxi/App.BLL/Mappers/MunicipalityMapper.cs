using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MunicipalityMapper : BaseMapper<App.BLL.DTO.AdminArea.MunicipalityDTO, MunicipalityDTO>
{
    public MunicipalityMapper(IMapper mapper) : base(mapper)
    {
    }
}