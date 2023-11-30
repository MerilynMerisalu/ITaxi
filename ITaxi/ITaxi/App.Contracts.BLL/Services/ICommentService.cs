using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICommentService: IEntityService<App.BLL.DTO.AdminArea.CommentDTO>,
    ICommentRepositoryCustom<App.BLL.DTO.AdminArea.CommentDTO>
{
    
}