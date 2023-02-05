using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IBookingRepository : IEntityRepository<BookingDTO>
{
    Task<IEnumerable<BookingDTO?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<BookingDTO?> GettingAllBookingsWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsAsync(Guid? userId = null, string? roleName = null, bool noTracking = true);
    IEnumerable<BookingDTO?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null, bool noTracking = true);

    Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsWithoutIncludesAsync( bool noTracking = true);
    // IEnumerable<Booking?> GettingAllOrderedBookingsWithoutIncludes( bool? noTracking = true);
    Task<BookingDTO?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    BookingDTO? GettingBookingWithoutIncludesById(Guid id,  bool noTracking = true);
    Task<List<BookingDTO>> SearchByCityAsync(string search, Guid? userId = null, string? roleName = null);
    string PickUpDateAndTimeStrFormat(BookingDTO booking);
    DateTime DateTimeFormatting();

    Task<BookingDTO?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null);
    BookingDTO? BookingDecline(Guid id, Guid? userId = null, string? roleName = null);
    Task<BookingDTO?> GettingBookingAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    BookingDTO? GettingBooking(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true);
    
}