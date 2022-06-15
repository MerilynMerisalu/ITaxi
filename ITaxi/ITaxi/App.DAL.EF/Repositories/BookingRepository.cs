using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BookingRepository: BaseEntityRepository<Booking, AppDbContext>, IBookingRepository
{
    public BookingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Booking> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }
        
        query = query.Include(b => b.City)
            .Include(b => b.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(b => b.Schedule)
            .Include(b => b.Vehicle)
            .ThenInclude(v => v!.VehicleMark)
            .Include(v => v.Vehicle)
            .ThenInclude(v => v!.VehicleModel)
            .Include(b => b.VehicleType)
            .Include(c => c.Drive)
            .ThenInclude(c => c!.Comment);
        return query;
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

    public async Task<IEnumerable<Booking?>>GettingAllOrderedBookingsAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking)
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

    public IEnumerable<Booking?> GettingAllOrderedBookings(bool noTracking = true)
    {
        return  CreateQuery(noTracking)
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

    public async Task<IEnumerable<Booking?>> GettingAllOrderedBookingsWithoutIncludesAsync(bool noTracking = true)
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

    public async Task<IEnumerable<Booking?>> SearchByCityAsync(string search)
    {
         var results =
            await RepoDbSet.Include(b => b.City)
                .Include(b => b.Driver)
                .ThenInclude(d => d!.AppUser)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .ThenInclude(v => v!.VehicleMark)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v!.VehicleModel)
                .Include(b => b.VehicleType)
                .Include(b => b.Drive)
                .ThenInclude(b => b!.Comment)
                .Where(b => b.City!.CityName.Contains(search)).ToListAsync();
         return results;
    }

    public string PickUpDateAndTimeStrFormat(Booking booking)
    {
        return booking.PickUpDateAndTime.ToLongDateString()
               + " " + booking.PickUpDateAndTime.ToShortTimeString();
    }

    public override async Task<Booking?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(b => b.Id.Equals(id));
    }

    public override Booking? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(b => b.Id.Equals(id));
    }
}