using System.Linq.Expressions;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DriveService: BaseEntityService<App.BLL.DTO.AdminArea.DriveDTO, App.DAL.DTO.AdminArea.DriveDTO, 
    IDriveRepository>, IDriveService
{
    public DriveService(IDriveRepository repository, IMapper<DriveDTO, DAL.DTO.AdminArea.DriveDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<DriveDTO>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GetAllDrivesWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriveDTO> GetAllDrivesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GetAllDrivesWithoutIncludes(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<DriveDTO>> GettingAllOrderedDrivesWithIncludesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedDrivesWithIncludesAsync(userId, roleName, noTracking))
            .Select(e => Mapper.Map(e))!;
    }


    

    public IEnumerable<DriveDTO> GettingAllOrderedDrivesWithIncludes(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GettingAllOrderedDrivesWithIncludes(userId, roleName, noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<DriveDTO?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingDriveWithoutIncludesAsync(id, noTracking));
    }

    public DriveDTO? GetDriveWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GetDriveWithoutIncludes(id, noTracking));
    }

    public async Task<IEnumerable<DriveDTO?>> SearchByDateAsync(DateTime search, Guid? userId = null, string? roleName = null)
    {
        return (await Repository.SearchByDateAsync(search, userId, roleName)).Select(e => Mapper.Map(e));
    }

    public IEnumerable<DriveDTO?> SearchByDate(DateTime search, Guid? userId = null, string? roleName = null)
    {
        return Repository.SearchByDate(search, userId, roleName).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<DriveDTO?>> PrintAsync(Guid? userId = null, string? roleName = null)
    {
        return (await Repository.PrintAsync(userId, roleName)).Select(e => Mapper.Map(e));
    }

    public IEnumerable<DriveDTO?> Print(Guid id)
    {
        return Repository.Print(id).Select(e => Mapper.Map(e));
    }

    public string PickUpDateAndTimeStr(DriveDTO drive)
    {
        return PickUpDateAndTimeStr(drive);
    }

    public async Task<IEnumerable<DriveDTO?>> GettingDrivesWithoutCommentAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingDrivesWithoutCommentAsync(userId, roleName, noTracking)).Select(e =>
            Mapper.Map(e));
    }

    public IEnumerable<DriveDTO?> GettingDrivesWithoutComment(bool noTracking = true)
    {
        return Repository.GettingDrivesWithoutComment(noTracking).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<DriveDTO?>> GettingAllDrivesForCommentsAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingAllDrivesForCommentsAsync(userId, roleName, noTracking))
            .Select(e => Mapper.Map(e));
    }

    public IEnumerable<DriveDTO?> GettingDrivesForComments(bool noTracking = true)
    {
        return Repository.GettingDrivesForComments(noTracking).Select(e => Mapper.Map(e));
    }

    public DriveDTO? AcceptingDrive(Guid id)
    {
        return Mapper.Map(Repository.AcceptingDrive(id));
    }

    public async Task<DriveDTO?> AcceptingDriveAsync(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.AcceptingDriveAsync(id, userId, roleName, noTracking, noIncludes));
    }

    public DriveDTO DecliningDrive(Guid id)
    {
        return Mapper.Map(Repository.DecliningDrive(id))!;
    }

    public async Task<DriveDTO?> DecliningDriveAsync(Guid id, 
        Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.DecliningDriveAsync(id, userId, roleName, noTracking, noIncludes));
    }

    public DriveDTO? StartingDrive(Guid id)
    {
        return Mapper.Map(Repository.StartingDrive(id));
    }

    public async Task<DriveDTO?> StartingDriveAsync(Guid id, Guid? userId = null, 
        string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
        return Mapper.Map(await Repository.StartingDriveAsync(id, userId, roleName, noTracking, noIncludes));
    }

    public DriveDTO? EndingDrive(Guid id)
    {
        return Mapper.Map(Repository.EndingDrive(id));
    }

    public async Task<DriveDTO?> EndingDriveAsync(Guid id, Guid? userId = null, 
        string? roleName = null, bool noTracking = true, bool noIncludes = true)
    {
        return Mapper.Map(await Repository.EndingDriveAsync(id, userId, roleName, noTracking, noIncludes));
    }

    public async Task<DriveDTO?> GettingFirstDriveAsync(Guid? id, Guid? userId = null,
        string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.GettingFirstDriveAsync(id, userId, roleName, noTracking, noIncludes));
    }

    public DriveDTO? GettingFirstDrive(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(Repository.GettingFirstDrive(id, userId, roleName, noTracking, noIncludes));
    }

    public async Task<DriveDTO?> GettingDriveByBookingIdAsync(Guid bookingId, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = true)
    {
        return Mapper.Map(
            await Repository.
                GettingDriveByBookingIdAsync(bookingId, userId, roleName
                    , noTracking, noIncludes));
    }

    public DriveDTO? GettingDriveByBookingId(Guid bookingId, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = true)
    {
        return Mapper.Map(
             Repository.
                 GettingDriveByBookingId(bookingId, userId, 
                     roleName, noTracking, noIncludes));
    }

    /*public async Task<DriveDTO?> GettingDriveByCommentIdAsync(Guid commentId
        , Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false)
    {
        return Mapper.Map(
            await Repository.GettingDriveByCommentIdAsync(commentId, userId, roleName
                , noTracking, noIncludes));
    }

    public DriveDTO? GettingDriveByCommentId(Guid commentId, 
        Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false)
    {
        return Mapper.Map(
            Repository.
                GettingDriveByCommentId(commentId, userId, 
                    roleName, noTracking, noIncludes));
    }*/


    public async Task<DriveDTO?> GettingDriveAsync(Guid bookingId, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.GettingDriveByBookingIdAsync(bookingId, userId, roleName, noTracking, noIncludes));
    }

    public DriveDTO? GettingDrive(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(Repository.GettingDriveByBookingId(id, userId, roleName, noTracking, noIncludes));
    }

    
}