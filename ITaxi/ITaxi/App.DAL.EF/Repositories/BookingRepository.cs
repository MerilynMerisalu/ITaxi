﻿using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using App.Enum.Enum;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BookingRepository : BaseEntityRepository<BookingDTO ,App.Domain.Booking, AppDbContext>, IBookingRepository
{
    public BookingRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.BookingDTO, App.Domain.Booking> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<BookingDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<BookingDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllBookingsWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<BookingDTO?> GettingAllBookingsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsAsync(Guid? userId = null,
       string? roleName = null, bool noTracking = true)
    {
        return (await CreateQuery(userId, roleName,noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.Customer!.AppUser!.LastName)
            .ThenBy(b => b.Customer!.AppUser!.FirstName)
            .ThenBy(b => b.City!.CityName)
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

   

    public IEnumerable<BookingDTO?> GettingAllOrderedBookings(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery(userId,roleName,noTracking)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.Customer!.AppUser!.LastName)
            .ThenBy(b => b.Customer!.AppUser!.FirstName)
            .ThenBy(b => b.City!.CityName)
            .ToList().Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<BookingDTO?>> GettingAllOrderedBookingsWithoutIncludesAsync(
        bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.NumberOfPassengers)
            .ThenBy(b => b.StatusOfBooking)
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

    

    public IEnumerable<BookingDTO?> GettingAllOrderedBookingsWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true)
            .OrderBy(b => b.PickUpDateAndTime.Date)
            .ThenBy(b => b.NumberOfPassengers)
            .ThenBy(b => b.StatusOfBooking)
            .ToList().Select(e =>Mapper.Map(e));
    }

    public async Task<BookingDTO?> GettingBookingWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefaultAsync(b => b.Id.Equals(id)));
    }

    public BookingDTO? GettingBookingWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefault(b => b.Id.Equals(id)));
    }

    public async Task<List<BookingDTO>> SearchByCityAsync(string search, Guid? userId = null,
        string? roleName = null)
    {
        var results = (await CreateQuery(userId, roleName)
            .Where(b => b.City!.CityName.Contains(search)).ToListAsync()
            ).Select(e => Mapper.Map(e));
        return results.ToList()!;
    }

    public string PickUpDateAndTimeStrFormat(BookingDTO booking)
    {
        return booking.PickUpDateAndTime.ToLongDateString() + " "
                                                            + booking.PickUpDateAndTime.ToShortTimeString();
    }

    public DateTime DateTimeFormatting()
    {
        return Convert.ToDateTime(DateTime.Now.ToString("g"));
    }

    

/// <summary>
/// Decline the booking, this will also decline the associated drive and set the times.
/// </summary>
/// <param name="id">Id of the booking to decline</param>
/// <param name="userId">Id of the current user</param>
/// <param name="roleName"></param>
/// <param name="noTracking"></param>
/// <param name="noIncludes"></param>
/// <returns></returns>
    public async Task<BookingDTO?> BookingDeclineAsync(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = true)
    {
        var booking = await RepoDbContext.Bookings.FirstOrDefaultAsync(x => x.Id == id);
        if (booking == null)
        {
            return null;
        }
        booking.StatusOfBooking = StatusOfBooking.Declined;
        booking.IsDeclined = true;
        booking.DeclineDateAndTime = DateTime.Now.ToUniversalTime();
        
        var user = await RepoDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId!.Value);
        booking.DeclinedBy = user!.Email;
        
        var drive = await RepoDbContext.Drives.FirstOrDefaultAsync(x => x.Id == booking.DriveId);
        if (drive != null)
        {
            drive.IsDriveDeclined = true;
            drive.StatusOfDrive = StatusOfDrive.Declined;
            drive.DriveDeclineDateAndTime = booking.DeclineDateAndTime;
            RepoDbContext.Drives.Update(drive);
        }
       
        await RepoDbContext.SaveChangesAsync();
        
        return await FirstOrDefaultAsync(id, userId, roleName, noTracking, noIncludes);
    }

    public BookingDTO? BookingDecline(Guid id, Guid? userId = null, string? roleName = null)
    {
        var booking = FirstOrDefault(id, userId, roleName);
        if (booking == null)
        {
            return null;
        }
        booking.StatusOfBooking = StatusOfBooking.Declined;
        return booking;
    }

    public async Task<BookingDTO?> GettingBookingAsync(Guid id, Guid? userId = null, 
        string? roleName = null, bool noIncludes = false,
        bool noTracking = true)
    {
        var booking = await FirstOrDefaultAsync(id, userId, roleName,noIncludes, noTracking);
        return booking;
    }

    public BookingDTO? GettingBooking(Guid id, Guid? userId = null, string? roleName = null, 
        bool noTracking = true, bool noIncludes = false)
    {
        var booking = FirstOrDefault(id, userId, roleName, noTracking, noIncludes);
        return booking;
    }

    public async Task<bool> HasAnyScheduleAsync(Guid id)
    {
        return await RepoDbContext.Bookings.AnyAsync(b => b.ScheduleId.Equals(id));
    }

    public bool HasAnySchedule(Guid id)
    {
        return RepoDbContext.Bookings.Any(b => b.ScheduleId.Equals(id));
    }

    public async Task<BookingDTO> GettingBookingByDriveIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noIncludes = true, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName,noTracking, noIncludes).SingleOrDefaultAsync(b => b.DriveId.Equals(id)))!;
    }

    public BookingDTO GettingBookingByDriveId(Guid id, Guid? userId = null, string? roleName = null,  bool noIncludes = true, bool noTracking = true)
    {
        return  Mapper.Map(CreateQuery(userId, roleName,noTracking, noIncludes).SingleOrDefault(b => b.DriveId.Equals(id)))!;
    }

    


    public  async Task<BookingDTO?> FirstOrDefaultAsync(Guid id,Guid? userId = null, string? 
        roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(userId, roleName,noTracking, noIncludes)
            .FirstOrDefaultAsync(b => b.Id.Equals(id)));
    }

    public  BookingDTO? FirstOrDefault(Guid id,Guid? userId = null, string? roleName = null, 
       bool noIncludes = false, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, roleName,noTracking, noIncludes)
            .FirstOrDefault(b => b.Id.Equals(id)));
    }

    public override BookingDTO Add(BookingDTO booking)
    {
        // Assign the Drive via the implicit related object creation
        booking.Drive = new DriveDTO
        {
            Id = new Guid(),
            DriverId = booking.DriverId,
            StatusOfDrive = StatusOfDrive.Awaiting
        };
        return base.Add(booking);
    }

    protected  IQueryable<Booking> CreateQuery(Guid? userId = null, string? roleName = null,bool noTracking = true, 
        bool noIncludes = false, bool showDeleted = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted);
        if (noTracking) query = query.AsNoTracking();
        if (noIncludes)
        {
            return query;
        }

        if (roleName is ("Admin"))
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
            .Where(u => u.Customer!.AppUserId.Equals(userId) || u.Driver!.AppUserId.Equals(userId)) ;
        return query;
    }
    
}