using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PhotoRepository: BaseEntityRepository<Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    
    protected override IQueryable<Photo> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.AppUser);
        return query;
    }


    public async Task<IEnumerable<Photo?>> GetAllPhotosWithIncludesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Photo?> GetAllPhotosWithIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public async Task<Photo?> GetPhotoByIdAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(p => p.Id.Equals(id));
    }

    public Photo? GetPhotoById(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(p => p.Id.Equals(id));
    }
}