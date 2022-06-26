using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IPhotoRepository: IEntityRepository<Photo>
{
    Task<IEnumerable<Photo?>> GetAllPhotosWithIncludesAsync(bool noTracking = true);
    IEnumerable<Photo?> GetAllPhotosWithIncludes(bool noTracking = true);
    Task<Photo?> GetPhotoByIdAsync(Guid id, bool noTracking = true);
    Photo? GetPhotoById(Guid id, bool noTracking = true);
}