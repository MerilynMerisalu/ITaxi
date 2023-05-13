using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PhotoRepository : BaseEntityRepository<PhotoDTO, App.Domain.Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.AdminArea.PhotoDTO, App.Domain.Photo> mapper)
        : base(dbContext, mapper)
    {
    }
    
    public async Task<IEnumerable<PhotoDTO?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, 
        string? roleName = null, bool noTracking = true)
    {
        return (await CreateQuery(userId,roleName,noTracking).ToListAsync())
            .Select(e => Mapper.Map(e));
    }

    public IEnumerable<PhotoDTO?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null, 
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking).ToList().Select(e => Mapper.Map(e));
    }

    public async Task<PhotoDTO?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName,noTracking)
            .FirstOrDefaultAsync(p => p.Id.Equals(id)));
    }

    public PhotoDTO? GetPhotoById(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName,noTracking)
            .FirstOrDefault(p => p.Id.Equals(id)));
    }


    protected  IQueryable<Photo> CreateQuery(Guid? userId = null, 
        string? roleName = null,
        bool noTracking = true,
        bool showDeleted = false)
    {
        var query = base.CreateQuery(noIncludes: false, noTracking: noTracking, showDeleted: showDeleted);
        if (noTracking) query = query.AsNoTracking();

        if (roleName != null && roleName.Equals(nameof(Admin)))
        {
            
            query = query.Include(c => c.AppUser);
            return query;
        }
       
        query = query.Include(c => c.AppUser).Where(p => p.AppUser!.Id.Equals(userId));
        return query;
    }
}