using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CommentMapper: BaseMapper<App.BLL.DTO.AdminArea.CommentDTO,
    App.DAL.DTO.AdminArea.CommentDTO>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }
}