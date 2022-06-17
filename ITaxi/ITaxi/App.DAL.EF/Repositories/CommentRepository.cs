using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CommentRepository : BaseEntityRepository<Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Comment> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.Drive)
            .ThenInclude(d => d!.Booking);
        ;
        return query;
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
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Comment> GetAllCommentsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }


    public async Task<IEnumerable<Comment>> GettingAllOrderedCommentsWithIncludesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .OrderBy(c => c.Drive!.Booking!.PickUpDateAndTime.Date)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Day)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Month)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Year)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Hour)
            .ThenBy(c => c.Drive!.Booking!.PickUpDateAndTime.Minute)
            .ThenBy(c => c.CommentText)
            .ToListAsync();

    }

    public IEnumerable<Comment> GettingAllOrderedCommentsWithIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking)
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
        return await CreateQuery(noTracking).OrderBy(c => c.CommentText).ToListAsync();
    }

    public IEnumerable<Comment> GettingAllOrderedCommentsWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(c => c.CommentText).ToList();
    }

    public async Task<Comment?> GettingCommentWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public Comment? GettingCommentWithoutIncludes(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id));
    }
}

    
  