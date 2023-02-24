using System.Collections;
using System.Linq.Expressions;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;
using Booking = App.Resources.Areas.App.Domain.AdminArea.Booking;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriveRepository : IEntityRepository<DriveDTO>, 
    IDriveRepositoryCustom<App.DAL.DTO.AdminArea.DriveDTO>
{
}

public interface IDriveRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllDrivesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity>> GettingAllOrderedDrivesWithIncludesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true);
    IEnumerable<TEntity> GettingAllOrderedDrivesWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<TEntity?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true);
    TEntity? GetDriveWithoutIncludes(Guid id, bool noTracking = true);
    Task<IEnumerable<TEntity?>> SearchByDateAsync(DateTime search, Guid? userId = null, string? roleName = null);
    IEnumerable<TEntity?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null);

    Task<IEnumerable<TEntity?>> PrintAsync( Guid? userId = null, string? roleName = null);
    IEnumerable<TEntity?> Print(Guid id);
    string PickUpDateAndTimeStr(TEntity drive);
    Task<IEnumerable<TEntity?>> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IEnumerable<TEntity?> GettingDrivesWithoutComment(bool noTracking = true);
    Task<IEnumerable<TEntity?>> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IEnumerable<TEntity?> GettingDrivesForComments(bool noTracking = true);
    TEntity? AcceptingDrive(Guid id);
    Task<TEntity?> AcceptingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true, 
        bool noIncludes = false);
    TEntity? DecliningDrive(Guid id);
    Task<TEntity?> DecliningDriveAsync
        (Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true, bool noIncludes = false);
    TEntity? StartingDrive(Guid id);
    Task<TEntity?> StartingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true,
        bool noIncludes = true);
    TEntity? EndingDrive(Guid id);
    Task<TEntity?> EndingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = true);
    Task<TEntity?> GettingFirstDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false );
    TEntity? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false);
    Task<TEntity?> GettingDriveAsync(Guid bookingId, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false);
    TEntity? GettingDrive(Guid bookingId, Guid? userId = null, string? roleName = null, 
        bool noTracking = true,bool noIncludes = false );
    
    
}