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
    [Area("AdminArea")]
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
                .Include(b => b.VehicleType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Bookings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.City)
                .Include(b => b.Customer)
                .Include(b => b.Drive)
                .Include(b => b.Driver)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .Include(b => b.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
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
                    .Where(v => v.DriverId.Equals(booking.DriverId))
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
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", booking.CityId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", booking.CustomerId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "Id", "Id", booking.DriveId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", booking.DriverId);
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", booking.ScheduleId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "VehiclePlateNumber", booking.VehicleId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "VehicleTypeName", booking.VehicleTypeId);
            return View(booking);
        }

        // POST: AdminArea/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ScheduleId,DriverId,CustomerId,VehicleTypeId,VehicleId,CityId,PickUpDateAndTime,PickupAddress,DestinationAddress,NumberOfPassengers,HasAnAssistant,AdditionalInfo,StatusOfBooking,DriveId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", booking.CityId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", booking.CustomerId);
            ViewData["DriveId"] = new SelectList(_context.Drives, "Id", "Id", booking.DriveId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", booking.DriverId);
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id", booking.ScheduleId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "VehiclePlateNumber", booking.VehicleId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "VehicleTypeName", booking.VehicleTypeId);
            return View(booking);
        }

        // GET: AdminArea/Bookings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.City)
                .Include(b => b.Customer)
                .Include(b => b.Drive)
                .Include(b => b.Driver)
                .Include(b => b.Schedule)
                .Include(b => b.Vehicle)
                .Include(b => b.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: AdminArea/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null) _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(Guid id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
