using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IBookingRepository: IEntityRepository<Booking>
{
    Task<IEnumerable<Booking?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Booking?> GettingAllBookingsWithoutIncludes(bool noTracking = true);

    Task<IEnumerable<Booking?>> GettingAllOrderedBookingsAsync(bool noTracking = true);
    IEnumerable<Booking?> GettingAllOrderedBookings(bool noTracking = true);

    Task<IEnumerable<Booking?>> GettingAllOrderedBookingsWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Booking?> GettingAllOrderedBookingsWithoutIncludes(bool noTracking = true);
    Task<Booking?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true);
    Booking? GettingBookingWithoutIncludesById(Guid id, bool noTracking = true);
    Task<IEnumerable<Booking?>> SearchByCityAsync(string search);
    string PickUpDateAndTimeStrFormat(Booking booking);

}