using System.Collections;
using System.Linq.Expressions;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriveRepository : IEntityRepository<DriveDTO>
{
    
    Task<IEnumerable<DriveDTO>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<DriveDTO> GetAllDrivesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<DriveDTO>> GettingAllOrderedDrivesWithIncludesAsync(Guid? userId = null, string? roleName = null, bool? noTracking = true);
    IEnumerable<DriveDTO> GettingAllOrderedDrivesWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);

    Task<DriveDTO?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true);
    DriveDTO? GetDriveWithoutIncludes(Guid id, bool noTracking = true);
    Task<IEnumerable<DriveDTO?>> SearchByDateAsync(DateTime search, Guid? userId = null, string? roleName = null);
    IEnumerable<DriveDTO?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null);

    Task<IEnumerable<DriveDTO?>> PrintAsync( Guid? userId = null, string? roleName = null);
    IEnumerable<DriveDTO?> Print(Guid id);
    string PickUpDateAndTimeStr(DriveDTO drive);
    Task<IEnumerable<DriveDTO?>> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IEnumerable<DriveDTO?> GettingDrivesWithoutComment(bool noTracking = true);
    Task<IEnumerable<DriveDTO?>> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IEnumerable<DriveDTO?> GettingDrivesForComments(bool noTracking = true);
    DriveDTO? AcceptingDrive(Guid id);
    Task<DriveDTO?> AcceptingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    DriveDTO? DecliningDrive(Guid id);
    Task<DriveDTO?> DecliningDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    DriveDTO? StartingDrive(Guid id);
    Task<DriveDTO?> StartingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true );
    DriveDTO? EndingDrive(Guid id);
    Task<DriveDTO?> EndingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<DriveDTO?> GettingFirstDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    DriveDTO? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);

    Task<DriveDTO?> GettingSingleOrDefaultDriveAsync(Expression<Func<Drive, bool>> filter, string? roleName = null,
        bool noTracking = true);

    DriveDTO? GettingSingleOrDefaultDrive( Expression<Func<Drive, bool>> filter, string? roleName = null,
        bool noTracking = true);

}