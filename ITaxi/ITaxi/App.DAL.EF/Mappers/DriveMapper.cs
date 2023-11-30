using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class DriveMapper: BaseMapper<DTO.AdminArea.DriveDTO, Domain.Drive>
{
    public DriveMapper(IMapper mapper) : base(mapper)
    {
    }
}