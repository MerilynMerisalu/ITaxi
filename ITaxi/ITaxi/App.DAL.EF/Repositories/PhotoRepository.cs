/*using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PhotoRepository : BaseEntityRepository<Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IEnumerable<Photo?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, 
        string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId,roleName,noTracking).ToListAsync();
    }

    public IEnumerable<Photo?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null, 
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking).ToList();
    }

    public async Task<Photo?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking).FirstOrDefaultAsync(p => p.Id.Equals(id));
    }

    public Photo? GetPhotoById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking).FirstOrDefault(p => p.Id.Equals(id));
    }


    protected  IQueryable<Photo> CreateQuery(Guid? userId = null, 
        string? roleName = null,
        bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName != null && roleName.Equals(nameof(Admin)))
        {
            
            query = query.Include(c => c.AppUser);
            return query;
        }
       
        query = query.Include(c => c.AppUser).Where(p => p.AppUser!.Id.Equals(userId));
        return query;
    }
}*/