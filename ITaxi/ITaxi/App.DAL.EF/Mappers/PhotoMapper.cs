using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PhotoMapper: BaseMapper<App.DAL.DTO.AdminArea.PhotoDTO, App.Domain.Photo>
{
    public PhotoMapper(IMapper mapper) : base(mapper)
    {
    }
}