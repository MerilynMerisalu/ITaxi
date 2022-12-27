using App.DAL.DTO.AdminArea;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CountyMapper:BaseMapper<App.BLL.DTO.AdminArea.CountyDTO, App.DAL.DTO.AdminArea.CountyDTO>
{
    public CountyMapper(IMapper mapper) : base(mapper)
    {
        
    }
}