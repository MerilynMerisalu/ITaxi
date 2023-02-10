using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IPhotoRepository : IEntityRepository<PhotoDTO>
{
    Task<IEnumerable<PhotoDTO?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, string? roleName = null, 
    bool noTracking = true);
    IEnumerable<PhotoDTO?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<PhotoDTO?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    PhotoDTO? GetPhotoById(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true);
}