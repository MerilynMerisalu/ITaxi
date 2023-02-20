using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PhotoMapper: BaseMapper<DTO.AdminArea.PhotoDTO, Domain.Photo>
{
    public PhotoMapper(IMapper mapper) : base(mapper)
    {
    }
}