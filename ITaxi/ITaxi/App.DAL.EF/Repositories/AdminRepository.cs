using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AdminRepository: BaseEntityRepository<Admin, AppDbContext>, IAdminRepository
{
    public AdminRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Admin> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(a => a.AppUser)
            .Include(a => a.City);
        return query;
    }

    public override IEnumerable<Admin> GetAll(bool noTracking = true)
    {
        var res = CreateQuery();
        return res.ToList();
    }

    public override async Task<IEnumerable<Admin>> GetAllAsync(bool noTracking = true)
    {
        var res = CreateQuery();
        return await res.ToListAsync();
    }

    public override async Task<Admin?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var res =  CreateQuery(noTracking);
        return await res.FirstOrDefaultAsync(a => a.Id.Equals(id));
    }

    public override Admin? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(a => a.Id.Equals(id));
    }

    public async Task<IEnumerable<Admin>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(a => a.AppUser!.LastName)
            .ThenBy(a => a.AppUser!.FirstName).ToListAsync();
    }

    public IEnumerable<Admin> GetAllAdminsOrderedByLastName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(a => a.AppUser!.LastName)
            .ThenBy(a => a.AppUser!.FirstName).ToList();
    }
}