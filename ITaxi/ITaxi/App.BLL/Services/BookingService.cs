using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class BookingService : BaseEntityService<App.BLL.DTO.AdminArea.BookingDTO,
    App.DAL.DTO.AdminArea.BookingDTO, IBookingRepository>, IBookingService
{
    public BookingService(IBookingRepository repository, IMapper<BookingDTO, DAL.DTO.AdminArea.BookingDTO> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllBookingsWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e));
    }

    public IEnumerable<BookingDTO?> GettingAllBookingsWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllBookingsWithoutIncludes(noTracking)
            .Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedBookingsAsync(userId, roleName, noTracking))
            .Select(e => Mapper.Map(e));
    }

    public IEnumerable<BookingDTO?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Repository.GettingAllOrderedBookings(userId, roleName, noTracking)
            .Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllBookingsWithoutIncludesAsync(noTracking))
            .Select(e => Mapper.Map(e));
    }

    public async Task<BookingDTO?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingBookingWithoutIncludesByIdAsync(id, noTracking));
    }

    public BookingDTO? GettingBookingWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingBookingWithoutIncludesById(id, noTracking));
    }

    public async Task<List<BookingDTO>> SearchByCityAsync(string search, Guid? userId = null, string? roleName = null)
    {
        return (await Repository.SearchByCityAsync(search, userId, roleName))
            .Select(e => Mapper.Map(e)).ToList()!;
    }

    public string PickUpDateAndTimeStrFormat(BookingDTO booking)
    {
        return Repository.PickUpDateAndTimeStrFormat(Mapper.Map(booking)!);
    }

    public DateTime DateTimeFormatting()
    {
        return Repository.DateTimeFormatting();
    }

    public async Task<BookingDTO?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null)
    {
        return Mapper.Map(await Repository.BookingDeclineAsync(id, userId, roleName));
    }

    public BookingDTO? BookingDecline(Guid id, Guid? userId = null, string? roleName = null)
    {
        return Mapper.Map(Repository.BookingDecline(id, userId, roleName));
    }

    public async Task<BookingDTO?> GettingBookingAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingBookingAsync(id, userId, roleName, noTracking));
    }

    public BookingDTO? GettingBooking(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(
            Repository.GettingBooking(id, userId, roleName, noTracking));
    }
}

