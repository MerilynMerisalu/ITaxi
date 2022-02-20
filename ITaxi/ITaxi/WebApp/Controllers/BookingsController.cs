#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Bookings.Include(b => b.City).Include(b => b.Customer).Include(b => b.Drive).Include(b => b.Driver).Include(b => b.Schedule).Include(b => b.Vehicle).Include(b => b.VehicleType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
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

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["DriveId"] = new SelectList(_context.Drives, "Id", "Id");
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "VehiclePlateNumber");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "VehicleTypeName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,DriverId,CustomerId,VehicleTypeId,VehicleId,CityId,PickUpDateAndTime,PickupAddress,DestinationAddress,NumberOfPassengers,HasAnAssistant,AdditionalInfo,StatusOfBooking,DriveId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.Id = Guid.NewGuid();
                _context.Add(booking);
                await _context.SaveChangesAsync();
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

        // GET: Bookings/Edit/5
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

        // POST: Bookings/Edit/5
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

        // GET: Bookings/Delete/5
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

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(Guid id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
