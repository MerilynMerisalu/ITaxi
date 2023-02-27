using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CommentRepository : BaseEntityRepository<CommentDTO, App.Domain.Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.CommentDTO, App.Domain.Comment> mapper) 
        : base(dbContext, mapper)
    {
    }


    public override async Task<IEnumerable<CommentDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e =>Mapper.Map(e))!;
    }

    public override IEnumerable<CommentDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<CommentDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(c => c.Id.Equals(id)));
    }

    public  CommentDTO? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id)));
    }


    public async Task<IEnumerable<CommentDTO>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CommentDTO> GetAllCommentsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e))!;
    }


    public async Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithIncludesAsync(
        Guid? userId= null, string? roleName = null,bool noTracking = true, bool noIncludes = false)
    {
        var res =(await CreateQuery(userId, roleName,noIncludes, noTracking)
            .OrderBy(c => c.Drive!.Booking!.PickUpDateAndTime.Date)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Day)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Month)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Year)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(c => c.CommentText)
            .ToListAsync()).Select(e => Mapper.Map(e))!;
        return res!;
    }

    public IEnumerable<CommentDTO> GettingAllOrderedCommentsWithIncludes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking)
            .OrderBy(c => c.Drive!.Booking!.PickUpDateAndTime.Date)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Day)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Month)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Year)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(c => c.CommentText)
            .ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CommentDTO>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking, noIncludes: false)
            .OrderBy(c => c.CommentText).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CommentDTO> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking, noIncludes: true)
            .OrderBy(c => c.CommentText).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<CommentDTO?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefaultAsync(c => c.Id.Equals(id)));
    }

    
    public CommentDTO? GettingCommentWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefault(c => c.Id.Equals(id)));
    }

    public string PickUpDateAndTimeStr(CommentDTO comment)
    {
        return comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
    }

    public async Task<CommentDTO?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null, 
        string? roleName = null, bool noIncludes = false,
        bool noTracking = true)
    {
        var res = Mapper.Map(await CreateQuery(userId, roleName, noIncludes , noTracking)
            .FirstOrDefaultAsync(c => c.Id.Equals(id)));
        return res;
    }

    

    public CommentDTO? GettingTheFirstComment(Guid id, Guid? userId = null, string? roleName = null,
        bool noIncludes = false,bool noTracking = false)
    {
        return Mapper.Map(CreateQuery(userId,roleName,noIncludes, noTracking)
            .FirstOrDefault(c => c.Id.Equals(id)));
    }

    public async Task<CommentDTO?> GettingCommentByDriveIdAsync(Guid driveId, Guid? userId = null, 
        string? roleName = null, bool noIncludes = true, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noIncludes, noTracking)
            .FirstOrDefaultAsync(c => c.Drive!.Booking!.Id.Equals(driveId)));
    }


    public CommentDTO? GettingCommentByDriveId(Guid driveId, 
        Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName, noIncludes, noTracking)
            .FirstOrDefault(c => c.Drive!.Booking!.Id.Equals(driveId)));
    }

    protected  IQueryable<Comment> CreateQuery(Guid? userId= null, string? roleName = null,
       bool noIncludes = false, bool noTracking = true)
    {
        
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();
        if (noIncludes == true )
        {
            return query;
        }

        if (roleName is "Admin")
        {
            query = query.Include(c => c.Drive)
                .ThenInclude(d => d!.Booking)
                .ThenInclude(d => d!.Customer)
                .ThenInclude(d => d!.AppUser)
                .Include(d => d.Drive)
                .ThenInclude(d => d!.Driver)
                .ThenInclude(d => d!.AppUser);
            query = query;
            return query;
        }

        
        query = query.Include(c => c.Drive)
            .ThenInclude(d => d!.Booking)
            .ThenInclude(d => d!.Customer)
            .ThenInclude(d => d!.AppUser)
            .Include(d => d.Drive)
            .ThenInclude(d => d!.Driver)
            .ThenInclude(d => d!.AppUser)
            .Where(c => c.Drive!.Booking!.Customer!.AppUserId.Equals(userId));

        return query;
    }
}