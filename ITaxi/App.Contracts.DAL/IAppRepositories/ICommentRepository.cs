using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICommentRepository : IEntityRepository<CommentDTO>,
    ICommentRepositoryCustom<App.DAL.DTO.AdminArea.CommentDTO>
{
}

public interface ICommentRepositoryCustom<TEntity> 
{
    Task<IEnumerable<TEntity>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllCommentsWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingAllOrderedCommentsWithIncludesAsync(Guid? userId = null, string? roleName = null
        ,bool noTracking = true, bool noIncludes = false);
    //IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true);
    Task<TEntity?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true);
    //Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true);
    string PickUpDateAndTimeStr(TEntity comment);
    Task<TEntity?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null, string? roleName = null,
         bool noIncludes = false, bool noTracking = true);
    TEntity? GettingTheFirstComment(Guid id, Guid? userId = null, string? roleName = null, 
        bool noIncludes = false, bool noTracking = true);
    Task<TEntity?> GettingCommentByDriveIdAsync(Guid driveId, Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = false);
    TEntity? GettingCommentByDriveId(Guid driveId, Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = false);
}