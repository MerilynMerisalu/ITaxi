using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AdminMapper: BaseMapper<DTO.AdminArea.AdminDTO, Domain.Admin>
{
    public AdminMapper(IMapper mapper) : base(mapper)
    {
    }
}