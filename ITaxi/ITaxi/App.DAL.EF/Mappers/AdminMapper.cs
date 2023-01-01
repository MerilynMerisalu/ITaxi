using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AdminMapper: BaseMapper<App.DAL.DTO.AdminArea.AdminDTO, App.Domain.Admin>
{
    public AdminMapper(IMapper mapper) : base(mapper)
    {
    }
}