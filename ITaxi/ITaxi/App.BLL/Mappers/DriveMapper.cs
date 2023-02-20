using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DriveMapper: BaseMapper<DTO.AdminArea.DriveDTO, App.DAL.DTO.AdminArea.DriveDTO>
{ 
    public DriveMapper(IMapper mapper) : base(mapper)
    {
    }
}