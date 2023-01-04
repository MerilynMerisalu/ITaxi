using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriverMapper: BaseMapper<App.DAL.DTO.AdminArea.DriverDTO, App.Domain.Driver>
{
    public DriverMapper(IMapper mapper) : base(mapper)
    {
    }
}