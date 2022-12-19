/*using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using App.Domain.Enum;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BookingRepository : BaseEntityRepository<Booking, AppDbContext>, IBookingRepository
{
    public BookingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Booking> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Booking?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Booking?> GettingAllBookingsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Booking?>> GettingAllOrderedBookingsAsync(Guid? userId = null,
       string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.PickUpDateAndTime.Day)
            .ThenBy(b => b.PickUpDateAndTime.Month)
            .ThenBy(b => b.PickUpDateAndTime.Year)
            .ThenBy(b => b.PickUpDateAndTime.Hour)
            .ThenBy(b => b.PickUpDateAndTime.Minute)
            .ThenBy(b => b.Customer!.AppUser!.LastName)
            .ThenBy(b => b.Customer!.AppUser!.FirstName)
            .ThenBy(b => b.City!.CityName)
            .ToListAsync();
    }

   

    public IEnumerable<Booking?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId,roleName,noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.PickUpDateAndTime.Day)
            .ThenBy(b => b.PickUpDateAndTime.Month)
            .ThenBy(b => b.PickUpDateAndTime.Year)
            .ThenBy(b => b.PickUpDateAndTime.Hour)
            .ThenBy(b => b.PickUpDateAndTime.Minute)
            .ThenBy(b => b.Customer!.AppUser!.LastName)
            .ThenBy(b => b.Customer!.AppUser!.FirstName)
            .ThenBy(b => b.City!.CityName)
            .ToList();
    }

    public async Task<IEnumerable<Booking?>> GettingAllOrderedBookingsWithoutIncludesAsync(
        bool noTracking = true)
    {
        return await base.CreateQuery(noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.PickUpDateAndTime.Day)
            .ThenBy(b => b.PickUpDateAndTime.Month)
            .ThenBy(b => b.PickUpDateAndTime.Year)
            .ThenBy(b => b.PickUpDateAndTime.Hour)
            .ThenBy(b => b.PickUpDateAndTime.Minute)
            .ThenBy(b => b.NumberOfPassengers)
            .ThenBy(b => b.StatusOfBooking)
            .ToListAsync();
    }

    

    public IEnumerable<Booking?> GettingAllOrderedBookingsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.PickUpDateAndTime.Day)
            .ThenBy(b => b.PickUpDateAndTime.Month)
            .ThenBy(b => b.PickUpDateAndTime.Year)
            .ThenBy(b => b.PickUpDateAndTime.Hour)
            .ThenBy(b => b.PickUpDateAndTime.Minute)
            .ThenBy(b => b.NumberOfPassengers)
            .ThenBy(b => b.StatusOfBooking)
            .ToList();
    }

    public async Task<Booking?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(b => b.Id.Equals(id));
    }

    public Booking? GettingBookingWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(b => b.Id.Equals(id));
    }

    public async Task<List<Booking>> SearchByCityAsync(string search, Guid? userId = null,
        string? roleName = null)
    {
        var results = await CreateQuery(userId, roleName)
            .Where(b => b.City!.CityName.Contains(search)).ToListAsync();
        return results;
    }

    public string PickUpDateAndTimeStrFormat(Booking booking)
    {
        return booking.PickUpDateAndTime.ToLongDateString() + " "
                                                            + booking.PickUpDateAndTime.ToShortTimeString();
    }

    public DateTime DateTimeFormatting()
    {
        return Convert.ToDateTime(DateTime.Now.ToString("g"));
    }


    public async Task<Booking?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null)
    {
        var booking = await FirstOrDefaultAsync(id, userId, roleName);
        if (booking == null)
        {
            return null;
        }
        booking.StatusOfBooking = StatusOfBooking.Declined;
        return booking;
    }

    public Booking? BookingDecline(Guid id, Guid? userId = null, string? roleName = null)
    {
        var booking = FirstOrDefault(id, userId, roleName);
        if (booking == null)
        {
            return null;
        }
        booking.StatusOfBooking = StatusOfBooking.Declined;
        return booking;
    }

    public async Task<Booking?> GettingBookingAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        var booking = await FirstOrDefaultAsync(id, userId, roleName, noTracking);
        return booking;
    }

    public Booking? GettingBooking(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var booking = FirstOrDefault(id, userId, roleName, noTracking);
        return booking;
    }

    public  async Task<Booking?> FirstOrDefaultAsync(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName,noTracking).FirstOrDefaultAsync(b => b.Id.Equals(id));
    }

    public  Booking? FirstOrDefault(Guid id,Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking).FirstOrDefault(b => b.Id.Equals(id));
    }

    protected  IQueryable<Booking> CreateQuery(Guid? userId = null, string? roleName = null,bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName is nameof(Admin))
        {
            query = query.Include(b => b.City)
                .Include(b => b.Driver)
                .ThenInclude(d => d!.AppUser)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .ThenInclude(v => v!.VehicleMark)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v!.VehicleModel)
                .Include(b => b.VehicleType!.VehicleTypeName)
                .ThenInclude(v => v.Translations)
                .Include(c => c.Drive)
                .ThenInclude(c => c!.Comment)
                .Include(b => b.Customer)
                .ThenInclude(c => c!.AppUser);
            return query;
        }
        query = query.Include(b => b.City)
            .Include(b => b.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(b => b.Schedule)
            .Include(b => b.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(b => b.VehicleType!.VehicleTypeName)
            .ThenInclude(v => v.Translations)
            .Include(c => c.Drive)
            .ThenInclude(c => c!.Comment)
            .Where(u => u.Customer!.AppUserId.Equals(userId));
        return query;
    }
}*/