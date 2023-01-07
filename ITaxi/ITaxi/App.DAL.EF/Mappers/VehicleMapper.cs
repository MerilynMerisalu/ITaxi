using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleMapper : BaseMapper<App.DAL.DTO.AdminArea.VehicleDTO, App.Domain.Vehicle>
{
    public VehicleMapper(IMapper mapper) : base(mapper)
    {
    }
}