using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IBookingRepository : IEntityRepository<Booking>
{
    Task<IEnumerable<Booking?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Booking?> GettingAllBookingsWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<Booking?>> GettingAllOrderedBookingsAsync(Guid? userId = null, string? roleName = null, bool noTracking = true);
    IEnumerable<Booking?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null, bool noTracking = true);

    Task<IEnumerable<Booking?>> GettingAllOrderedBookingsWithoutIncludesAsync(bool noTracking = true);
    // IEnumerable<Booking?> GettingAllOrderedBookingsWithoutIncludes( bool? noTracking = true);
    Task<Booking?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    Booking? GettingBookingWithoutIncludesById(Guid id,  bool noTracking = true);
    Task<List<Booking>> SearchByCityAsync(string search, Guid? userId = null, string? roleName = null);
    string PickUpDateAndTimeStrFormat(Booking booking);
    DateTime DateTimeFormatting();

    Task<Booking?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null);
    Booking? BookingDecline(Guid id, Guid? userId = null, string? roleName = null);
    Task<Booking?> GettingBookingAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    Booking? GettingBooking(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    
}