using App.Contracts.DAL.IAppRepositories;

using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AppUserRepository : BaseEntityRepository<App.DAL.DTO.Identity.AppUser, App.Domain.Identity.AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Identity.AppUser, App.Domain.Identity.AppUser> mapper)
        : base(dbContext, mapper)
    {
    }

    public override IEnumerable<DTO.Identity.AppUser> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public override async Task<IEnumerable<DTO.Identity.AppUser>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override async Task<DTO.Identity.AppUser?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes).FirstOrDefaultAsync(e => e.Id.Equals(id)));
    }

    public override DTO.Identity.AppUser? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DTO.Identity.AppUser>> GetAllAppUsersOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DTO.Identity.AppUser> GetAllAppUsersOrderedByLastName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName).ToList().Select(e => Mapper.Map(e))!;
    }

    protected override IQueryable<Domain.Identity.AppUser> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();
        //if (!noIncludes)
        //    query = query.Include(a => a.AppUser)
        //                 .Include(a => a.City);
        return query;
    }
}