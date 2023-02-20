using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CommentMapper: BaseMapper<DTO.AdminArea.CommentDTO, Domain.Comment>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }
}