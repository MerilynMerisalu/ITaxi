using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICommentRepository : IEntityRepository<Comment>
{
    Task<IEnumerable<Comment>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Comment> GetAllCommentsWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithIncludesAsync(bool noTracking = true);
    IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(bool noTracking = true);
    Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Comment> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true);
    Task<Comment?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true);
    Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true);
}