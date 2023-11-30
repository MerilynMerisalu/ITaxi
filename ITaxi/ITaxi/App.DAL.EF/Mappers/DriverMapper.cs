using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverMapper: BaseMapper<DTO.AdminArea.DriverDTO, Domain.Driver>
{
    public DriverMapper(IMapper mapper) : base(mapper)
    {
    }
}