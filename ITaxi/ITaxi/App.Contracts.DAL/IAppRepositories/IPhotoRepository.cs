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
    Task<int> GetPhotoCountByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool isCountingDeleted = false);
    int GetPhotoCountByVehicleId(Guid vehicleId, Guid? userId = null,
        string? roleName = null,
        bool noTracking = true,
        bool isCountingDeleted = false);
    Task<string?> GetDirectoryIdByVehicleIdAsStringAsync(Guid vehicleId, Guid? userId = null,
        string? roleName = null,
        bool noTracking = true,
        bool noIncludes = false);
    string? GetDirectoryIdByVehicleIdAsString(Guid vehicleId, Guid? userId = null,
       string? roleName = null,
       bool noTracking = true,
       bool noIncludes = false);
    Task<List<TEntity?>>? GetAllPhotosByVehicleIdWithIncludesAsync(Guid vehicleId, 
        Guid? userId = null,
       string? roleName = null,
       bool noTracking = true,
       bool noIncludes = false);
  List<TEntity?>? GetAllPhotosByVehicleIdWithIncludes(Guid vehicleId,
        Guid? userId = null,
       string? roleName = null,
       bool noTracking = true,
       bool noIncludes = false);

    Task<string?> GetDirectoryIdByAppUserIdAsStringAsync(Guid userId, string? roleName,
        bool noTracking=true, bool noIncludes = false);
    string? GetDirectoryIdByAppUserIdAsString(Guid userId, string? roleName,
        bool noTracking = true, bool noIncludes = false);
    Task<bool> IsPhotoOfVehicleAsync(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    bool IsPhotoOfVehicle(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<bool> IsPhotoOfAdminAsync(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    bool IsPhotoOfAdmin(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<string?> GetAdminFirstAndLastNameAsync(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    string? GetAdminFirstAndLastName(Guid photoId, Guid adminId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<string?> GetVehicleIdentifierAsync(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    string? GetVehicleIdentifier(Guid photoId, Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<bool> IsPhotoOfDriverAsync(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    bool IsPhotoOfDriver(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<string?> GetDriverFirstAndLastNameAsync(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    string? GetDriverFirstAndLastName(Guid photoId, Guid driverId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<bool> IsPhotoOfCustomerAsync(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    bool IsPhotoOfCustomer(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    Task<string?> GetCustomerFirstAndLastNameAsync(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    string? GetCustomerFirstAndLastName(Guid photoId, Guid customerId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false, bool showDeleted = false);
    
}