/*using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CommentRepository : BaseEntityRepository<Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public override async Task<IEnumerable<Comment>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Comment> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override async Task<Comment?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public override Comment? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id));
    }


    public async Task<IEnumerable<Comment>> GetAllCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking, noIncludes: true).ToListAsync();
    }

    public IEnumerable<Comment> GetAllCommentsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList();
    }


    public async Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithIncludesAsync(Guid? userId= null, string? roleName = null,bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking)
            .OrderBy(c => c.Drive!.Booking!.PickUpDateAndTime.Date)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Day)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Month)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Year)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(c => c.CommentText)
            .ToListAsync();
    }

    public IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking)
            .OrderBy(c => c.Drive!.Booking!.PickUpDateAndTime.Date)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Day)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Month)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Year)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(c => c.CommentText)
            .ToList();
    }

    public async Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithoutIncludesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking, noIncludes: true).OrderBy(c => c.CommentText).ToListAsync();
    }

    public IEnumerable<Comment> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking, noIncludes: true).OrderBy(c => c.CommentText).ToList();
    }

    public async Task<Comment?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking, noIncludes: true).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).FirstOrDefault(c => c.Id.Equals(id));
    }

    public string PickUpDateAndTimeStr(Comment comment)
    {
        return comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
    }

    public async Task<Comment?> GettingTheFirstCommentAsync(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public Comment? GettingTheFirstComment(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = false)
    {
        return CreateQuery(userId, roleName, noTracking)
            .FirstOrDefault(c => c.Id.Equals(id));
    }

    protected  IQueryable<Comment> CreateQuery(Guid? userId= null, string? roleName = null, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        if (roleName is nameof(Admin))
        {
            query = query.Include(c => c.Drive)
                .ThenInclude(d => d!.Booking)
                .ThenInclude(d => d!.Customer)
                .ThenInclude(d => d!.AppUser)
                .Include(d => d.Drive)
                .ThenInclude(d => d!.Driver)
                .ThenInclude(d => d!.AppUser);

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
}*/