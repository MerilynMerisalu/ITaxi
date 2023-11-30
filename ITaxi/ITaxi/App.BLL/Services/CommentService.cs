using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CommentService: BaseEntityService<App.BLL.DTO.AdminArea.CommentDTO, 
    App.DAL.DTO.AdminArea.CommentDTO, ICommentRepository>, ICommentService
{
    public CommentService(ICommentRepository repository, IMapper<CommentDTO, DAL.DTO.AdminArea.CommentDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CommentDTO>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GetAllCommentsWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CommentDTO> GetAllCommentsWithoutIncludes(bool noTracking = true)
    {
        return Repository.GetAllCommentsWithoutIncludes(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithIncludesAsync(Guid?
        userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GettingAllOrderedCommentsWithIncludesAsync(userId, roleName, noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedCommentsWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CommentDTO> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllOrderedCommentsWithoutIncludes(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<CommentDTO?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingCommentWithoutIncludesAsync(id, noTracking));
    }

    public string PickUpDateAndTimeStr(CommentDTO comment)
    {
        return Repository.PickUpDateAndTimeStr(Mapper.Map(comment)!);
    }

    public async Task<CommentDTO?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null,
        string? roleName = null, bool noIncludes = false, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingTheFirstCommentAsync(id, userId, roleName, noIncludes, noTracking));
    }

    public CommentDTO? GettingTheFirstComment(Guid id, Guid? userId = null, 
        string? roleName = null, bool noIncludes = false, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingTheFirstComment(id, userId, roleName, noIncludes, noTracking));
    }

    public async Task<CommentDTO?> GettingCommentByDriveIdAsync(Guid driveId, Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = true)
    {
        return Mapper.Map(
            await Repository.GettingCommentByDriveIdAsync(driveId, userId, roleName, noIncludes, noTracking));
    }

    public CommentDTO? GettingCommentByDriveId(Guid driveId, Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingCommentByDriveId(driveId, userId, roleName, noIncludes, noTracking));
    }
}