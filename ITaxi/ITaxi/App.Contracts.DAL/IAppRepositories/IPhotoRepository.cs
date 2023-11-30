using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IPhotoRepository : IEntityRepository<PhotoDTO>, 
    IPhotoRepositoryCustom<App.DAL.DTO.AdminArea.PhotoDTO>
{
    
}

public interface IPhotoRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllPhotosWithIncludesAsync(Guid? userId = null, string? roleName = null, 
        bool noTracking = true);
    IEnumerable<TEntity?> GetAllPhotosWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<TEntity?> GetPhotoByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    TEntity? GetPhotoById(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true);
}