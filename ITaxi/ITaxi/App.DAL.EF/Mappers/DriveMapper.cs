using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriveMapper: BaseMapper<App.DAL.DTO.AdminArea.DriveDTO, App.Domain.Drive>
{
    public DriveMapper(IMapper mapper) : base(mapper)
    {
    }
}