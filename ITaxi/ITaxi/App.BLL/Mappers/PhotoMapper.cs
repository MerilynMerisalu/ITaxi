using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PhotoMapper: BaseMapper<App.BLL.DTO.AdminArea.PhotoDTO, 
    App.DAL.DTO.AdminArea.PhotoDTO>
{
    public PhotoMapper(IMapper mapper) : base(mapper)
    {
    }
}