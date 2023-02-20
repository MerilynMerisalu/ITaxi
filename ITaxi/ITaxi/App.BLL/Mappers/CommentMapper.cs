using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CommentMapper: BaseMapper<DTO.AdminArea.CommentDTO,
    App.DAL.DTO.AdminArea.CommentDTO>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }
}