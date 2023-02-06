using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICommentRepository : IEntityRepository<CommentDTO>
{
   Task<IEnumerable<CommentDTO>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true);
   IEnumerable<CommentDTO> GetAllCommentsWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithIncludesAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    //IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(bool noTracking = true);
    Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<CommentDTO> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true);
    Task<CommentDTO?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true);
    //Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true);
    string PickUpDateAndTimeStr(Comment comment);
    Task<CommentDTO?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    CommentDTO? GettingTheFirstComment(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
}