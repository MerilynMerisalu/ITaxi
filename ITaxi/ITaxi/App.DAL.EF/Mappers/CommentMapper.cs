using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CommentMapper: BaseMapper<App.DAL.DTO.AdminArea.CommentDTO, App.Domain.Comment>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }
}