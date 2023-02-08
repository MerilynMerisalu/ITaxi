using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DriveMapper: BaseMapper<App.BLL.DTO.AdminArea.DriveDTO, App.DAL.DTO.AdminArea.DriveDTO>
{ 
    public DriveMapper(IMapper mapper) : base(mapper)
    {
    }
}