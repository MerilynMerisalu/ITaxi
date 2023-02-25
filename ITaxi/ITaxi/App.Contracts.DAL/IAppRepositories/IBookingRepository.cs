using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IBookingRepository : IEntityRepository<BookingDTO>, 
    IBookingRepositoryCustom<App.DAL.DTO.AdminArea.BookingDTO>
{
    
}

public interface IBookingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity?> GettingAllBookingsWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<TEntity?>> GettingAllOrderedBookingsAsync(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    IEnumerable<TEntity?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null,
        bool noTracking = true);

    Task<IEnumerable<TEntity?>> GettingAllOrderedBookingsWithoutIncludesAsync(bool noTracking = true);

    // IEnumerable<Booking?> GettingAllOrderedBookingsWithoutIncludes( bool? noTracking = true);
    Task<TEntity?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    TEntity? GettingBookingWithoutIncludesById(Guid id, bool noTracking = true);
    Task<List<TEntity>> SearchByCityAsync(string search, Guid? userId = null, string? roleName = null);
    string PickUpDateAndTimeStrFormat(TEntity booking);
    DateTime DateTimeFormatting();

    Task<TEntity?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null);
    TEntity? BookingDecline(Guid id, Guid? userId = null, string? roleName = null);
    Task<TEntity?> GettingBookingAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false);
    TEntity? GettingBooking(Guid bookingId, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false);
    Task<bool> HasAnyScheduleAsync(Guid id);
    bool HasAnySchedule(Guid id);
    Task<TEntity> GettingBookingByDriveIdAsync(Guid id, Guid? userId = null, 
        string? roleName = null, bool noIncludes = true, bool noTracking = true);
    TEntity GettingBookingByDriveId(Guid id, Guid? userId = null, 
        string? roleName = null, bool noIncludes = true, bool noTracking = true);

}