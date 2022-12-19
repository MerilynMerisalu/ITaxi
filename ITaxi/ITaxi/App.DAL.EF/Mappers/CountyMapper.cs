using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CountyMapper:BaseMapper<App.DTO.AdminArea.CountyDTO,App.Domain.County>
{
    public CountyMapper(IMapper mapper) : base(mapper)
    {
    }
}