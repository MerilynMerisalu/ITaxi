#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;
using WebApp.Models.Enum;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Bookings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Bookings.
                Include(b => b.City)
                .Include(b => b.Driver)
                .ThenInclude(d => d.AppUser)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(b => b.VehicleType)
                .Include(c => c.Drive)
                .ThenInclude(c => c.Comment);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Bookings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteBookingViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.City)
                .Include(b => b.Driver)
                .ThenInclude(d => d.AppUser)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(b => b.VehicleType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            vm.Id = booking.Id;
            vm.City = booking.City!.CityName;
            vm.LastAndFirstName = booking.Driver!.AppUser!.LastAndFirstName;
            vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
            vm.AdditionalInfo = booking.AdditionalInfo;
            vm.DestinationAddress = booking.DestinationAddress;
            vm.PickupAddress = booking.PickupAddress;
            vm.VehicleType = booking.VehicleType!.VehicleTypeName;
            vm.HasAnAssistant = booking.HasAnAssistant;
            vm.NumberOfPassengers = booking.NumberOfPassengers;
            vm.StatusOfBooking = booking.StatusOfBooking;
            vm.PickUpDateAndTime = booking.PickUpDateAndTime;

            return View(vm);
        }

        // GET: AdminArea/Bookings/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditBookingViewModel();
            vm.Cities = new SelectList(await _context.Cities.Select(c => new {c.Id, c.CityName}).ToListAsync(),
                nameof(City.Id), nameof(City.CityName));
            vm.VehicleTypes = new SelectList(await _context.VehicleTypes
                    .Select(v => new {v.Id, v.VehicleTypeName}).ToListAsync(),
                nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName));
            vm.PickUpDateAndTime = Convert.ToDateTime(DateTime.Now.ToString("g"));
            return View(vm);
        }

        // POST: AdminArea/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditBookingViewModel vm)
        {
            var booking = new Booking();
            if (ModelState.IsValid)
            {
                booking.Id = Guid.NewGuid();
                booking.CityId = vm.CityId;
                booking.CustomerId = await _context.Customers.Select(c => c.Id).FirstOrDefaultAsync();
                booking.DriverId = await _context.Drivers.Select(d => d.Id).FirstOrDefaultAsync();
                booking.ScheduleId = await _context.Schedules
                    .Where(s => s.DriverId.Equals(booking.DriverId))
                    .Select(s => s.Id).FirstOrDefaultAsync();
                booking.VehicleId = await _context.Vehicles
                    .Where(v => v.DriverId.Equals(booking.DriverId)
                                && v.VehicleAvailability == VehicleAvailability.Available)
                    .Select(v => v.Id).FirstOrDefaultAsync();
                booking.AdditionalInfo = vm.AdditionalInfo;
                booking.DestinationAddress = vm.DestinationAddress;
                booking.PickupAddress = vm.PickupAddress;
                booking.VehicleTypeId = vm.VehicleTypeId;
                booking.HasAnAssistant = vm.HasAnAssistant;
                booking.NumberOfPassengers = vm.NumberOfPassengers;
                booking.StatusOfBooking = StatusOfBooking.Awaiting;
                booking.PickUpDateAndTime = vm.PickUpDateAndTime;
                _context.Add(booking);

                var drive = new Drive()
                {
                    Id = new Guid(),
                    Booking = booking,
                    DriverId = booking.DriverId
                };

                await _context.Drives.AddAsync(drive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Bookings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditBookingViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.Id.Equals(id));
            if (booking == null)
            {
                return NotFound();
            }

            
            vm.Cities = new SelectList(await _context.Cities.Select(c => new {c.Id, c.CityName}).ToListAsync()
                , nameof(City.Id), nameof(City.CityName));
            vm.AdditionalInfo = booking.AdditionalInfo;
            vm.CityId = booking.CityId;
            vm.DestinationAddress = booking.DestinationAddress;
            vm.PickupAddress = booking.PickupAddress;
            vm.VehicleTypes = new SelectList(
                await _context.VehicleTypes.Select(v => new {v.Id, v.VehicleTypeName}).ToListAsync(),
                nameof(VehicleType.Id)
                , nameof(VehicleType.VehicleTypeName));
            vm.HasAnAssistant = booking.HasAnAssistant;
            vm.NumberOfPassengers = booking.NumberOfPassengers;
            vm.VehicleTypeId = booking.VehicleTypeId;
            vm.PickUpDateAndTime = booking.PickUpDateAndTime;
            return View(vm);
        }

        // POST: AdminArea/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditBookingViewModel vm)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.Id.Equals(id));
            if (booking != null && id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (booking != null)
                    {
                        booking.Id = id;
                        booking.CityId = vm.CityId;
                        booking.CustomerId = await _context.Customers.Select(c => c.Id).FirstOrDefaultAsync();
                        booking.DriverId = await _context.Drivers.Select(d => d.Id).FirstOrDefaultAsync();
                        booking.ScheduleId = await _context.Schedules
                            .Where(s => s.DriverId.Equals(booking.DriverId))
                            .Select(s => s.Id).FirstOrDefaultAsync();
                        booking.VehicleId = await _context.Vehicles
                            .Where(v => v.DriverId.Equals(booking.DriverId) 
                            && v.VehicleAvailability == VehicleAvailability.Available)
                            .Select(v => v.Id).FirstOrDefaultAsync();
                        booking.AdditionalInfo = vm.AdditionalInfo;
                        booking.DestinationAddress = vm.DestinationAddress;
                        booking.PickupAddress = vm.PickupAddress;
                        booking.VehicleTypeId = vm.VehicleTypeId;
                        booking.HasAnAssistant = vm.HasAnAssistant;
                        booking.NumberOfPassengers = vm.NumberOfPassengers;
                        booking.StatusOfBooking = StatusOfBooking.Awaiting;
                        booking.PickUpDateAndTime = vm.PickUpDateAndTime;

                        
                            _context.Bookings.Update(booking);

                    }
                    var drive = await _context.Drives
                        .SingleOrDefaultAsync(d => d.Booking.Id.Equals(booking.Id));
                    if (drive != null)
                    {
                        if (booking != null)
                        {
                            drive.DriverId = booking.DriverId;
                            drive.Booking = booking;
                        }

                        _context.Drives.Update(drive);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (booking != null && !BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Bookings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteBookingViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.City)
                .Include(b => b.Driver)
                .ThenInclude(d => d.AppUser)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(b => b.VehicleType)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
            
            if (booking == null)
            {
                return NotFound();
            }
            vm.Id = booking.Id;
            vm.City = booking.City!.CityName;
            vm.LastAndFirstName = booking.Driver!.AppUser!.LastAndFirstName;
            vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
            vm.AdditionalInfo = booking.AdditionalInfo;
            vm.DestinationAddress = booking.DestinationAddress;
            vm.PickupAddress = booking.PickupAddress;
            vm.VehicleType = booking.VehicleType!.VehicleTypeName;
            vm.HasAnAssistant = booking.HasAnAssistant;
            vm.NumberOfPassengers = booking.NumberOfPassengers;
            vm.StatusOfBooking = booking.StatusOfBooking;
            vm.PickUpDateAndTime = booking.PickUpDateAndTime;

            return View(booking);
        }

        // POST: AdminArea/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.Id.Equals(id));
            var drive = await _context.Drives.SingleOrDefaultAsync(d => d.Booking.Id.Equals(id));
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.DriveId.Equals(drive.Id));
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            if (drive != null)
            {
                _context.Drives.Remove(drive);
            }
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(Guid id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        /// <summary>
        /// Search records by city name
        /// </summary>
        /// <param name="search">City name</param>
        /// <returns>An index view with search results</returns>
        [HttpPost]
        public async Task<IActionResult> SearchByCityAsync([FromForm] string search)
        {
            var results =
                 await _context.Bookings.Include(b => b.City)
                     .Include(b => b.Driver)
                     .ThenInclude(d => d.AppUser)
                     .Include(b => b.Schedule)
                     .Include(b => b.Vehicle)
                     .ThenInclude(v => v.VehicleMark)
                     .Include(v => v.Vehicle)
                     .ThenInclude(v => v.VehicleModel)
                     .Include(b => b.VehicleType)
                     .Where(b => b.City.CityName.Contains(search)).ToListAsync();
            return View(nameof(Index), results);
        }
    }
}
