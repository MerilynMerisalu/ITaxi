using System.Collections;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDriveRepository : IEntityRepository<Drive>
{
    
    Task<IEnumerable<Drive>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Drive> GetAllDrivesWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Drive>> GettingAllOrderedDrivesWithIncludesAsync(Guid? userId = null, string? roleName = null, bool? noTracking = true);
    IEnumerable<Drive> GettingAllOrderedDrivesWithIncludes(Guid? userId = null, string? roleName = null,bool noTracking = true);

    Task<Drive?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true);
    Drive? GetDriveWithoutIncludes(Guid id, bool noTracking = true);
    Task<IEnumerable<Drive?>> SearchByDateAsync(DateTime search, Guid? userId = null, string? roleName = null);
    IEnumerable<Drive?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null);

    Task<IEnumerable<Drive?>> PrintAsync( Guid? userId = null, string? roleName = null);
    IEnumerable<Drive?> Print(Guid id);
    string PickUpDateAndTimeStr(Drive drive);
    Task<IList> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IList GettingDrivesWithoutComment(bool noTracking = true);
    Task<IList> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null,bool noTracking = true);
    IList GettingDrivesForComments(bool noTracking = true);
    Drive? AcceptingDrive(Guid id);
    Task<Drive?> AcceptingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    Drive? DecliningDrive(Guid id);
    Task<Drive?> DecliningDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    Drive? StartingDrive(Guid id);
    Task<Drive?> StartingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true );

    Drive? EndingDrive(Guid id);
    Task<Drive?> EndingDriveAsync(Guid id, Guid? userId = null, string? roleName = null,bool noTracking = true);
    Task<Drive?> GettingFirstDriveAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    Drive? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);

}