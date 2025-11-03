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

    public async Task<int> GetPhotoCountByVehicleIdAsync(Guid vehicleId, 
        Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool isCountingDeleted = false)
    {
        if (isCountingDeleted == false)
        {
            return (await CreateQuery(userId, roleName, noTracking)
            .CountAsync(p => p.VehicleId.Equals(vehicleId) && p.IsDeleted == false));
        }
        else
        {
            return (await CreateQuery(userId, roleName, noTracking)
            .CountAsync(p => p.VehicleId.Equals(vehicleId) && p.IsDeleted == true));
        }

    }

    public int GetPhotoCountByVehicleId(Guid vehicleId, Guid? userId = null,
        string? roleName = null, bool noTracking = true, bool isCountingDeleted = false)
    {
        if (isCountingDeleted == false)
        {
            return CreateQuery(userId, roleName, noTracking)
            .Count(p => p.VehicleId.Equals(vehicleId) && p.IsDeleted == false);
        }
        else
        {
            return CreateQuery(userId, roleName, noTracking)
            .Count(p => p.VehicleId.Equals(vehicleId) && p.IsDeleted == true);
        }
    }
        


    protected  IQueryable<Photo> CreateQuery(Guid? userId = null, 
        string? roleName = null,
        bool noIncludes = false,
        bool noTracking = true,
        bool showDeleted = false)
    {
        var query = base.CreateQuery(noIncludes: noIncludes, noTracking: noTracking, showDeleted: showDeleted);
        if (noTracking) query = query.AsNoTracking();

        if (roleName != null && roleName.Equals(nameof(Admin)))
        {

            query = query.Include(c => c.AppUser).Include(c => c.Vehicle)
                .ThenInclude(c => c.VehicleType).ThenInclude(c => c.VehicleTypeName)
                .ThenInclude(c => c.Translations)
                .Include(c => c.Vehicle).ThenInclude(c => c.VehicleMark)
                .Include(c => c.Vehicle).ThenInclude(c => c.VehicleModel)
                .Include(c => c.Driver).ThenInclude(c => c.AppUser);
            return query;
        }
       
        query = query.Include(c => c.AppUser).Where(p => p.AppUser!.Id.Equals(userId));
        return query;
    }

    public async Task<string?> GetDirectoryIdByVehicleIdAsStringAsync(Guid vehicleId,
        Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = (await CreateQuery(userId, roleName, noTracking, noIncludes).
            FirstOrDefaultAsync(p => p.VehicleId.Equals(vehicleId)));
        return result?.DirectoryTitleId ?? null;
    }

    public string? GetDirectoryIdByVehicleIdAsString(Guid vehicleId, Guid? userId = null, 
        string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
       var result = CreateQuery(userId, roleName, noTracking, noIncludes).
            FirstOrDefault(v => v.VehicleId.Equals(vehicleId));
        return result?.DirectoryTitleId ?? null;
    }

    public async Task<IEnumerable<PhotoDTO?>>?GetAllPhotosByVehicleIdWithIncludesAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = (await 
            CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes:noIncludes)
            .Where(p => p.VehicleId.Equals(vehicleId)).OrderBy(p => p.FileNameInDirectory).ToListAsync());
        return result.Select(p => Mapper.Map(p));
    }

    public IEnumerable<PhotoDTO?> GetAllPhotosByVehicleIdWithIncludes(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
       var result = 
            CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .Where(p => p.VehicleId.Equals(vehicleId)).OrderBy(p => p.FileNameInDirectory).ToList();
        return result.Select(p => Mapper.Map(p));
    }

    public async Task<string?> GetDirectoryIdByAppUserIdAsStringAsync(Guid userId, string? roleName, bool noTracking = true, bool noIncludes = false)
    {
        var result
            = await CreateQuery(userId: userId, roleName: roleName, noTracking:noTracking, noIncludes: noIncludes).FirstOrDefaultAsync(p  => p.AppUserId.Equals(userId) && p.DirectoryTitleId == null);
        return result!.DirectoryTitleId;
    }

    public string? GetDirectoryIdByAppUserIdAsString(Guid userId, string? roleName, bool noTracking = true, bool noIncludes = false)
    {
        var result
            =  CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes).FirstOrDefault(p => p.AppUserId.Equals(userId) && p.DirectoryTitleId == null);
        return result!.DirectoryTitleId;
    }
}