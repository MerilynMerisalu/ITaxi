using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CountyMapper:BaseMapper<CountyDTO,Domain.County>
{
    public CountyMapper(IMapper mapper) : base(mapper)
    {
        
    }
}