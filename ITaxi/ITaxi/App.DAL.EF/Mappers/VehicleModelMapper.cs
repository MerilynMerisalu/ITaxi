using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class VehicleModelMapper : BaseMapper<App.DAL.DTO.AdminArea.VehicleModelDTO, App.Domain.VehicleModel>
{
    public VehicleModelMapper(IMapper mapper) : base(mapper)
    {
    }
}