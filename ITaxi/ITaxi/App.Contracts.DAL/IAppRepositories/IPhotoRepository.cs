using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IPhotoRepository : IEntityRepository<Photo>
{
    
    Task<IEnumerable<Photo?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, string? roleName = null, 
    bool noTracking = true);
    IEnumerable<Photo?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<Photo?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    Photo? GetPhotoById(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true);
}